using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu_UIState : AUIState
{
    #region PROPERTIES
    VisualElement _menu;

    Button _playButton,
        _settingsButton,
        _quitButton;
    #endregion

    #region CONSTRUCTOR
    public MainMenu_UIState(UIDocument uiDocument)
    : base("MainMenu", uiDocument) { }
    #endregion

    #region INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _menu = _screen.Q<VisualElement>("Menu");
        _playButton = _screen.Q<Button>("PlayButton");
        _settingsButton = _screen.Q<Button>("SettingsButton");
        _quitButton = _screen.Q<Button>("QuitButton");

        if (_menu == null)
            Debug.LogWarning("_menu not found");
        if (_playButton == null)
            Debug.LogWarning("_playButton not found");
        if (_settingsButton == null)
            Debug.LogWarning("_settingsButton not found");
        if (_quitButton == null)
            Debug.LogWarning("_quitButton not found");
    }

    protected override void RegisterUICallbacksOnAwake()
    {
        _playButton.RegisterCallback<ClickEvent>(OnPlayClicked);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
        _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
    }

    public override void ExitState()
    {
        // Hide buttons when exiting main menu
        _menu.style.display = DisplayStyle.None;

        base.ExitState();
    }
    #endregion

    #region CALLBACK METHODS
    void OnPlayClicked(ClickEvent evt)
    {
        _gameManager.SwitchToGamePlayState();
        _soundManager.PlayButtonClickSFX();
    }

    void OnSettingsClicked(ClickEvent evt)
    {
        _uiManager.SwitchToSettingsMenuState();
        _soundManager.PlayButtonClickSFX();
    }

    void OnQuitClicked(ClickEvent evt)
    {
        _soundManager.PlayButtonClickSFX();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For convenience in the editor
#endif
    }

    public void OnMainMenuSceneLoadedFully()
    {
        _menu.style.display = DisplayStyle.Flex; // Show menu buttons
    }
    #endregion
}