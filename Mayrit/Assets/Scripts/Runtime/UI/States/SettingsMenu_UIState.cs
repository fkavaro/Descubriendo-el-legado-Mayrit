using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenu_UIState : AUIState
{
    #region PROPERTIES
    Button _closeButton;
    Switch _edgeScrollingSwitch,
        _showControlsSwitch;
    Slider _musicVolumeSlider,
        _sfxVolumeSlider;
    #endregion

    #region CONSTRUCTOR
    public SettingsMenu_UIState(UIDocument uiDocument)
    : base("SettingsMenu", uiDocument) { }
    #endregion

    #region OVERRIDDEN METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _closeButton = _screen.Q<Button>("CloseButton");
        _edgeScrollingSwitch = _screen.Q<Switch>("EdgeScrollingSwitch");
        _showControlsSwitch = _screen.Q<Switch>("ShowControlsSwitch");
        _musicVolumeSlider = _screen.Q<Slider>("MusicVolumeSlider");
        _sfxVolumeSlider = _screen.Q<Slider>("SFXVolumeSlider");

        if (_closeButton == null)
            Debug.LogWarning("_closeButton not found");
        if (_edgeScrollingSwitch == null)
            Debug.LogWarning("_edgeScrollingSwitch not found");
        if (_showControlsSwitch == null)
            Debug.LogWarning("_showControlsSwitch not found");
        if (_musicVolumeSlider == null)
            Debug.LogWarning("_musicVolumeSlider not found");
        if (_sfxVolumeSlider == null)
            Debug.LogWarning("_sfxVolumeSlider not found");
    }

    protected override void RegisterUICallbacksOnAwake()
    {
        _closeButton.RegisterCallback<ClickEvent>(OnCloseClicked);
        _musicVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnMusicVolumeChanged);
        _sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnSFXVolumeChanged);

        _edgeScrollingSwitch.Toggled += OnEdgeScrollingToggled;
        _showControlsSwitch.Toggled += OnShowControlsToggled;
    }

    public override void StartState()
    {
        base.StartState();

        _musicVolumeSlider.value = _soundManager.MusicVolume;
        _sfxVolumeSlider.value = _soundManager.EffectsVolume;
        _showControlsSwitch.Value = _uiManager.ControlsVisibilityValueSet;
        _edgeScrollingSwitch.Value = _uiManager.EdgeScrollingValueSet;
    }
    #endregion

    #region CALLBACK METHODS
    void OnCloseClicked(ClickEvent evt)
    {
        base.ExitState();
        _soundManager.PlayButtonClickSFX();

        if (_gameManager.IsInMainMenuState)
            _uiManager.SwitchToMainMenuState();
        else if (_gameManager.IsInPauseState)
            _uiManager.SwitchToPauseState();
    }

    void OnEdgeScrollingToggled(bool value)
    {
        _uiManager.InvokeEdgeScrollingToggledEvent(value);
        _soundManager.PlayButtonClickSFX();
    }

    void OnShowControlsToggled(bool value)
    {
        _uiManager.SetControlsVisibility(value);
        _soundManager.PlayButtonClickSFX();
    }

    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        _uiManager.InvokeMusicVolumeChangedEvent(evt.newValue);
    }

    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        _uiManager.InvokeSFXVolumeChangedEvent(evt.newValue);
    }
    #endregion
}
