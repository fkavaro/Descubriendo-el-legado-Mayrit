using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu_UIState : AUIState
{
    #region PROPERTIES
    Button _newGameButton,
        _loadGameButton,
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
        _newGameButton = _screen.Q<Button>("NewGameButton");
        _loadGameButton = _screen.Q<Button>("LoadGameButton");
        _settingsButton = _screen.Q<Button>("SettingsButton");
        _quitButton = _screen.Q<Button>("QuitButton");

        if (_newGameButton == null)
            Debug.LogWarning("_newGameButton not found");
        if (_loadGameButton == null)
            Debug.LogWarning("_loadGameButton not found");
        if (_settingsButton == null)
            Debug.LogWarning("_settingsButton not found");
        if (_quitButton == null)
            Debug.LogWarning("_quitButton not found");
    }

    protected override void RegisterUICallbacksOnAwake()
    {
        _newGameButton.RegisterCallback<ClickEvent>(OnNewGameClicked);
        _loadGameButton.RegisterCallback<ClickEvent>(OnLoadGameClicked);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
        _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
    }

    public override void StartState()
    {
        CheckLoadButtonAvailability();

        base.StartState();
    }
    #endregion

    void CheckLoadButtonAvailability()
    {
        // Check if game can be loaded to enable/disable Load Game button
        bool canLoadGame = GameSaveSystem.IsThereStoredData();
        _loadGameButton.SetEnabled(canLoadGame);
    }

    #region CALLBACK METHODS
    void OnNewGameClicked(ClickEvent evt)
    {
        GameSaveSystem.Clear();
        _gameManager.SwitchToGamePlayState();
        _soundManager.PlayButtonClickSFX();
    }

    void OnLoadGameClicked(ClickEvent evt)
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
    #endregion
}