using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenu_UIState : AUIState
{
    #region PROPERTIES
    Button _closeButton,
        _resetTutorialButton;

    Switch _edgeScrollingSwitch,
        _POIsVisualizationSwitch,
        _displayControlsMappingsSwitch,
        _milestoneSkipSwitch;

    Slider _musicVolumeSlider,
        _sfxVolumeSlider;

    public event Action CloseClickedEvent;
    public event Action<bool> EdgeScrollingToggledEvent;
    public event Action<bool> ShowPOIsToggledEvent;
    public event Action<bool> ShowControlsToggledEvent;
    public event Action<bool> MilestoneSkipToggledEvent;
    public event Action<float> MusicVolumeChangedEvent;
    public event Action<float> SFXVolumeChangedEvent;
    #endregion

    #region CONSTRUCTOR
    public SettingsMenu_UIState(UISystem uiSystem, UIDocument uiDocument, float fadeInDuration, float fadeOutDuration)
    : base(uiSystem, "SettingsMenu", uiDocument, fadeInDuration, fadeOutDuration) { }
    #endregion

    #region OVERRIDDEN METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _closeButton = GetButtonAndRegisterCallback("CloseButton", OnCloseClicked);

        _edgeScrollingSwitch = GetSwitchAndRegisterCallback("EdgeScrollingSwitch", OnEdgeScrollingToggled);
        _POIsVisualizationSwitch = GetSwitchAndRegisterCallback("ShowPOIsSwitch", OnShowPOIsToggled);
        _displayControlsMappingsSwitch = GetSwitchAndRegisterCallback("ShowControlsSwitch", OnShowControlsToggled);
        _milestoneSkipSwitch = GetSwitchAndRegisterCallback("MilestoneSkipSwitch", OnMilestoneSkipToggled);

        _musicVolumeSlider = GetSliderAndRegisterCallback("MusicVolumeSlider", OnMusicVolumeChanged);
        _sfxVolumeSlider = GetSliderAndRegisterCallback("SFXVolumeSlider", OnSFXVolumeChanged);

        _resetTutorialButton = GetButtonAndRegisterCallback("ResetTutorialButton", OnResetTutorialClicked);
    }

    public override void StartState()
    {
        base.StartState();

        _edgeScrollingSwitch.SetWithoutEvent(_gameManager.IsEdgeScrollingMovementEnabled);
        _POIsVisualizationSwitch.SetWithoutEvent(_gameManager.ArePOIsVisualized);
        _displayControlsMappingsSwitch.SetWithoutEvent(_gameManager.AreControlsMappingsDisplayed);
        _milestoneSkipSwitch.SetWithoutEvent(_gameManager.CanSkipMilestones);

        _musicVolumeSlider.value = _soundSystem.MusicVolume;
        _sfxVolumeSlider.value = _soundSystem.EffectsVolume;

        _resetTutorialButton.SetEnabled(GameSaveSystem.LoadTutorialCompletion());
    }
    #endregion

    #region CALLBACK METHODS
    void OnCloseClicked(ClickEvent evt)
    {
        base.ExitState();
        _soundSystem.PlayButtonClickSFX();
        CloseClickedEvent?.Invoke();
    }

    void OnEdgeScrollingToggled(bool newValue)
    {
        _soundSystem.PlayButtonClickSFX();
        EdgeScrollingToggledEvent?.Invoke(newValue);
    }

    void OnShowPOIsToggled(bool newValue)
    {
        _soundSystem.PlayButtonClickSFX();
        ShowPOIsToggledEvent?.Invoke(newValue);
    }

    void OnShowControlsToggled(bool newValue)
    {
        _soundSystem.PlayButtonClickSFX();
        ShowControlsToggledEvent?.Invoke(newValue);
    }

    void OnMilestoneSkipToggled(bool newValue)
    {
        _soundSystem.PlayButtonClickSFX();
        MilestoneSkipToggledEvent?.Invoke(newValue);
    }

    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        MusicVolumeChangedEvent?.Invoke(evt.newValue);
    }

    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        SFXVolumeChangedEvent?.Invoke(evt.newValue);
    }

    void OnResetTutorialClicked(ClickEvent evt)
    {
        GameSaveSystem.SaveTutorial(false);
        _resetTutorialButton.SetEnabled(false);
    }
    #endregion
}
