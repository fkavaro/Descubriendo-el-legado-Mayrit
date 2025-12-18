using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#region ENUMS
public enum MusicType
{
    None,
    MenuMusic,
    GameplayMusic,
}

public enum SFXType
{
    None,
    UI_ButtonClick,
    UI_TourStart,
    UI_TourEnd,
    Camera_Change
}
#endregion

#region STRUCTS
[Serializable]
public struct MusicList
{
    [SerializeField] public MusicType _type;
    [SerializeField] public List<AudioClip> _sounds;
}

[Serializable]
public struct SFXlist
{
    [SerializeField] public SFXType _type;
    [SerializeField] public List<AudioClip> _sounds;
}
#endregion

/// <summary>
/// Handles music and sound effects reproduction. 
/// Requires two audioSource components 
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [SerializeField] private AudioSource _effectsSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField, Range(0, 1)] private float _effectsVolume = 1f;
    [SerializeField, Range(0, 1)] private float _musicVolume = 1f;
    [SerializeField, Range(0, 1)] private float _musicFadeDuration = 0.5f;
    [SerializeField, Range(0, 2)] private float _resumeGuardSeconds = 0.25f;
    [SerializeField] private List<MusicList> _musicLists = new();
    [SerializeField] private List<SFXlist> _SFXLists = new();
    #endregion

    // Playlist state
    // Current active playlist type; None when stopped
    private MusicType _currentMusicType = MusicType.None;
    // Background coroutine that watches for track end and advances
    private Coroutine _playlistCoroutine;
    // Per-type shuffled queues used to play tracks sequentially without repeats
    private readonly Dictionary<MusicType, Queue<AudioClip>> _musicQueues = new();
    // True while app focus is lost or app is paused; prevents unintended advancing
    private bool _suspendAutoAdvance = false;
    // After regaining focus, ignore auto-advance checks for a brief window
    private float _ignoreAdvanceUntilTime = 0f;
    // True while a fade transition is in progress; avoids volume overrides
    private bool _isFadingMusic = false;

    #region LIFE CYCLE
#if UNITY_EDITOR
    void OnEnable()
    {
        MusicType[] musicTypes = (MusicType[])Enum.GetValues(typeof(MusicType));
        SFXType[] sfxTypes = (SFXType[])Enum.GetValues(typeof(SFXType));

        // Ensure all music types are represented in the list
        foreach (MusicType type in musicTypes)
        {
            if (type == MusicType.None)
                continue;
            if (!_musicLists.Exists(list => list._type == type))
                _musicLists.Add(new MusicList { _type = type, _sounds = new() });
        }

        // Ensure all SFX types are represented in the list
        foreach (SFXType type in sfxTypes)
        {
            if (type == SFXType.None)
                continue;
            if (!_SFXLists.Exists(list => list._type == type))
                _SFXLists.Add(new SFXlist { _type = type, _sounds = new() });
        }

    }
#endif

    void Start()
    {
        if (_effectsSource == null || _musicSource == null)
        {
            Debug.LogError("SoundManager: AudioSource references are missing!");
            return;
        }

        _effectsSource.loop = false;
        _musicSource.loop = false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // When focus is lost in the editor/player, audio may pause.
        // Prevents the playlist loop from advancing to the next track in that case.
        _suspendAutoAdvance = !hasFocus;
        if (hasFocus)
        {
            // Give audio a brief moment to resume before checking isPlaying
            _ignoreAdvanceUntilTime = Time.unscaledTime + _resumeGuardSeconds;
        }
    }

    void OnApplicationPause(bool paused)
    {
        // Same protection when app pauses/resumes (mobile, editor options, etc.)
        _suspendAutoAdvance = paused;
        if (!paused)
        {
            _ignoreAdvanceUntilTime = Time.unscaledTime + _resumeGuardSeconds;
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
            return;// To avoid error in editor

        // Update volumes in case they were changed in the inspector
        if (_effectsVolume != _effectsSource.volume)
            _effectsSource.volume = _effectsVolume;
        // Avoid overriding fade-in/out transitions
        if (!_isFadingMusic && _musicVolume != _musicSource.volume)
            _musicSource.volume = _musicVolume;
    }
    #endregion

    #region MUSIC METHODS
    /// <summary>
    /// Starts the menu music playlist. Tracks are shuffled, then played sequentially.
    /// </summary>
    public void PlayMenuMusic(float volume = 1)
    {
        PlayMusic(MusicType.MenuMusic, volume);
    }

    /// <summary>
    /// Starts the gameplay music playlist. Tracks are shuffled, then played sequentially.
    /// </summary>
    public void PlayGameplayMusic(float volume = 1)
    {
        PlayMusic(MusicType.GameplayMusic, volume);
    }

    /// <summary>
    /// Starts or switches the active music playlist to <paramref name="type"/> with the target volume.
    /// </summary>
    private void PlayMusic(MusicType type, float volume = 1)
    {
        // Return if type is none
        if (type == MusicType.None)
            return;

        // Set target volume immediately
        _musicVolume = volume;

        // Update current playlist type; reset current clip if switching type
        if (_currentMusicType != type)
        {
            _currentMusicType = type;
            _musicSource.Stop();
            _musicSource.clip = null;
        }

        // Ensure at least the first track starts now
        PlayNextInPlaylistInternal();

        // Start/ensure the auto-advance loop (only during play mode)
        if (Application.isPlaying && _playlistCoroutine == null)
            _playlistCoroutine = StartCoroutine(PlaylistLoop());
    }

    /// <summary>
    /// Pauses the current music track. Does not change playlist state.
    /// </summary>
    public void PauseMusic()
    {
        _musicSource.Pause();
    }

    /// <summary>
    /// Stops music playback and clears playlist state.
    /// </summary>
    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
        _currentMusicType = MusicType.None;

        if (_playlistCoroutine != null)
        {
            StopCoroutine(_playlistCoroutine);
            _playlistCoroutine = null;
        }
    }

    /// <summary>
    /// Updates the target music volume. If a fade is in progress, the fade will reach this value.
    /// </summary>
    public void UpdateMusicVolume(float volume)
    {
        _musicVolume = volume;
        _musicSource.volume = volume;
    }
    #endregion

    #region PLAYLIST HELPERS
    /// <summary>
    /// Dequeues and plays the next track from the current playlist. Uses fade if configured.
    /// </summary>
    private void PlayNextInPlaylistInternal()
    {
        if (_currentMusicType == MusicType.None)
            return;

        var queue = EnsureQueue(_currentMusicType);
        if (queue.Count == 0)
        {
            Debug.LogWarning($"No audio clips found for music type {_currentMusicType}");
            return;
        }

        var nextClip = queue.Dequeue();
        if (nextClip == null)
            return;

        // If a track is already playing and fade is enabled, perform a crossfade.
        if (_musicFadeDuration > 0f && _musicSource.clip != null && _musicSource.isPlaying)
        {
            // Smooth transition to the next track
            StartCoroutine(CrossfadeToClip(nextClip));
        }
        else
        {
            _musicSource.Stop();
            _musicSource.clip = nextClip;
            _musicSource.volume = _musicVolume;
            _musicSource.Play();
        }
    }

    /// <summary>
    /// Ensures a shuffled queue exists for the given type and refills it when empty.
    /// </summary>
    private Queue<AudioClip> EnsureQueue(MusicType type)
    {
        if (!_musicQueues.TryGetValue(type, out var queue))
        {
            queue = new Queue<AudioClip>();
            _musicQueues[type] = queue;
        }

        if (queue.Count == 0)
        {
            var clips = GetMusicClips(type);
            if (clips.Count == 0)
                return queue; // stays empty; caller will warn
            // Build a fresh shuffled queue
            var temp = new List<AudioClip>(clips);
            Shuffle(temp);
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] != null)
                    queue.Enqueue(temp[i]);
            }
        }

        return queue;
    }

    /// <summary>
    /// Returns the configured clips for a music type from the inspector lists.
    /// </summary>
    private List<AudioClip> GetMusicClips(MusicType type)
    {
        // Find the list entry for the type; if missing, return empty list
        int idx = _musicLists.FindIndex(list => list._type == type);
        if (idx < 0)
            return new List<AudioClip>();
        return _musicLists[idx]._sounds ?? new List<AudioClip>();
    }

    /// <summary>
    /// Fisher–Yates shuffle.
    /// </summary>
    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// Watches the music source and advances only when a track actually ends.
    /// Respects focus/pause guards to avoid unintended skipping on resume.
    /// </summary>
    private IEnumerator PlaylistLoop()
    {
        // Runs while a music type is active; advances when current clip ends
        while (_currentMusicType != MusicType.None)
        {
            // Skip advancing while focus is lost or immediately after resume
            if (_suspendAutoAdvance || Time.unscaledTime < _ignoreAdvanceUntilTime)
            {
                yield return null;
                continue;
            }

            // Advance only when the current clip actually ended
            if (_musicSource.clip != null && !_musicSource.isPlaying)
            {
                bool ended = _musicSource.timeSamples >= (_musicSource.clip.samples - 1);
                if (ended)
                {
                    PlayNextInPlaylistInternal();
                }
            }
            yield return null;
        }

        _playlistCoroutine = null;
    }
    #endregion

    #region TRANSITIONS
    /// <summary>
    /// Performs a simple crossfade on the same AudioSource:
    /// first fades out the current clip, swaps to the next, then fades in.
    /// </summary>
    private IEnumerator CrossfadeToClip(AudioClip nextClip)
    {
        if (nextClip == null)
            yield break;

        _isFadingMusic = true;

        float duration = Mathf.Max(0.0001f, _musicFadeDuration);
        float half = duration * 0.5f;

        // Fade out current
        float startVol = _musicSource.volume;
        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float percent = Mathf.Clamp01(t / half);
            _musicSource.volume = Mathf.Lerp(startVol, 0f, percent);
            yield return null;
        }

        // Swap clip
        _musicSource.Stop();
        _musicSource.clip = nextClip;
        _musicSource.volume = 0f;
        _musicSource.Play();

        // Fade in to target
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float percent = Mathf.Clamp01(t / half);
            _musicSource.volume = Mathf.Lerp(0f, _musicVolume, percent);
            yield return null;
        }

        _musicSource.volume = _musicVolume;
        _isFadingMusic = false;
    }

    /// <summary>
    /// Skips to the next track in the current playlist.
    /// If a fade is in progress, it is cancelled before skipping.
    /// </summary>
    public void SkipToNextTrack()
    {
        // Manual skip ignores guard; if fading, stop fade first
        if (_isFadingMusic)
            StopAllCoroutines();
        _playlistCoroutine = StartCoroutine(PlaylistLoop());
        PlayNextInPlaylistInternal();
    }
    #endregion

    #region SOUND EFFECT METHODS
    /// <summary>
    /// Plays a UI button click sound effect.
    /// </summary>
    public void PlayButtonClickSFX(float volume = 1)
    {
        PlaySFX(SFXType.UI_ButtonClick, volume);
    }

    /// <summary>
    /// Plays a tour start sound effect.
    /// </summary>
    public void PlayTourStartSFX(float volume = 1)
    {
        PlaySFX(SFXType.UI_TourStart, volume);
    }

    /// <summary>
    /// Plays a tour end sound effect.
    /// </summary>
    public void PlayTourEndSFX(float volume = 1)
    {
        PlaySFX(SFXType.UI_TourEnd, volume);
    }

    /// <summary>
    /// Plays a random sound of the specified SFX type using <see cref="AudioSource.PlayOneShot(AudioClip,float)"/>.
    /// </summary>
    public void PlaySFX(SFXType type, float volume = 1)
    {
        // Return if type is none
        if (type == SFXType.None)
            return;

        // Takes all the clips of the type
        List<AudioClip> clips = _SFXLists.Find(list => list._type == type)._sounds;

        if (clips.Count == 0)
        {
            Debug.LogWarning($"No audio clips found for sound type {type}");
            return;
        }

        // Randomly selects a clip from the list
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];


        // Played in effectsSource
        _effectsSource.PlayOneShot(randomClip, volume);
    }

    /// <summary>
    /// Stops any currently playing sound effect.
    /// </summary>
    public void StopSFX()
    {
        _effectsSource.Stop();
        _effectsSource.clip = null;
    }

    /// <summary>
    /// Updates the target SFX volume immediately.
    /// </summary>
    public void UpdateSFXVolume(float volume)
    {
        _effectsVolume = volume;
        _effectsSource.volume = volume;
    }
    #endregion
}