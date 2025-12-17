using UnityEngine;
using UnityEngine.UIElements;

public class HeritageMenu_UIState : AUIState
{
    #region PROPERTIES
    Button _playButton;
    #endregion

    #region CONSTRUCTOR
    public HeritageMenu_UIState(UIDocument uiDocument)
    : base("HeritageMenu", uiDocument) { }
    #endregion

    #region INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _playButton = _screen.Q<Button>("PlayButton");

        if (_playButton == null)
            Debug.LogWarning("_playButton not found");
    }

    protected override void RegisterUICallbacksOnAwake()
    {
        _playButton.RegisterCallback<ClickEvent>(OnPlayClicked);
    }

    public override void StartState()
    {
        base.StartState();
        _gameManager.SwitchToPauseState();
    }
    #endregion

    #region CALLBACK METHODS
    void OnPlayClicked(ClickEvent evt)
    {
        _uiManager.BehaviourSystem.SwitchToPreviousStateInStack(); // Player or spectator HUD
        _gameManager.SwitchToGamePlayState();
        _soundManager.PlayButtonClickSFX();
    }
    #endregion
}
