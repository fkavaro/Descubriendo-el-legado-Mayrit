using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CollectiblesManager : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [SerializeField] CollectiblesTracker _currentTracker;
    [SerializeField] Collectible _nextCollectible;
    [SerializeField] List<CollectibleSO> _allCollectiblesSOs = new();
    [SerializeField] int _totalCollectiblesCount;
    [SerializeField] int _foundCollectiblesCount = 0;
    #endregion

    #region INTERNAL PROPERTIES
    ScenesController _scenesController;
    SoundManager _soundManager;
    ProgressManager _progressManager;
    #endregion

    #region LIFE CYCLE
    void Awake()
    {
        ServiceLocator.Instance.Register(this);
    }

    void Start()
    {
        _totalCollectiblesCount = _allCollectiblesSOs.Count;

        _scenesController = ServiceLocator.Instance.Get<ScenesController>();
        _soundManager = ServiceLocator.Instance.Get<SoundManager>();
        _progressManager = ServiceLocator.Instance.Get<ProgressManager>();

        _scenesController.SceneLoadedPartiallyEvent += OnSceneLoadedPartially;

    }

    void OnDisable()
    {
        DetachFromTracker();

        ServiceLocator.Instance.Unregister(this);
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS
    void AttachToTracker(CollectiblesTracker tracker)
    {
        if (tracker == null)
        {
            Debug.LogWarning($"[CollectibleManager] Can't attach to null tracker");
            return;
        }
        if (_currentTracker == tracker)
        {
            Debug.LogWarning($"[CollectibleManager] Already attached to this tracker", tracker);
            return;
        }

        // Detach previous
        DetachFromTracker();

        // Update current
        _currentTracker = tracker;
        _nextCollectible = _currentTracker.NextCollectible;
        _currentTracker.OnCollectibleFoundEvent += OnCollectibleFound;
    }

    void DetachFromTracker()
    {
        _nextCollectible = null;
        if (_currentTracker == null) return;

        _currentTracker.OnCollectibleFoundEvent -= OnCollectibleFound;
    }
    #endregion

    #region CALLBACK METHODS
    void OnSceneLoadedPartially(SceneDatabase.SceneType type, SceneDatabase.SceneName name)
    {
        // Milestone loaded: attach to its tour
        if (type == SceneDatabase.SceneType.Milestone)
        {
            AttachToTracker(ServiceLocator.Instance.Get<CollectiblesTracker>());

            //TODO: Set found collectibles count based on progress
            //tracker.SetFoundCollectibles = _progressManager.FoundCollectibles;
        }
    }

    private void OnCollectibleFound(Collectible collectible)
    {
        if (collectible.Info == null)
        {
            Debug.LogWarning($"Collectible {collectible.name} has no CollectibleSO assigned. Ignoring.");
            return;
        }

        // CollectibleSO not in list, ignore
        if (!_allCollectiblesSOs.Contains(collectible.Info))
        {
            Debug.LogWarning($"Collectible {collectible.name} with SO {collectible.Info.Data.Header} not in the list of all collectibles. Ignoring.");
            return;
        }

        Debug.Log($"[Manager] Collectible found: {collectible.Info.Data.Header}");

        //TODO: change to custom SFX
        _soundManager.PlayTourEndSFX();
        _foundCollectiblesCount++;
        Debug.Log($"[Manager] Found {_foundCollectiblesCount}/{_totalCollectiblesCount} collectibles.");

        _nextCollectible = _currentTracker.NextCollectible;

        if (_nextCollectible != null)
            Debug.Log($"[Manager] Next collectible: {_nextCollectible.Info.Data.Header}");
        else
            Debug.Log($"[Manager] No more collectibles to find.");
    }
    #endregion
}
