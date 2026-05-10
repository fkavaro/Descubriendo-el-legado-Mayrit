using System;
using UnityEngine;
using UnityEngine.UIElements;
public class CreditsScreen_UIState : AUIState
{
    Button _closeButton;

    public CreditsScreen_UIState(UIDocument uiDocument, float fadeInDuration, float fadeOutDuration)
    : base("CreditsScreen", uiDocument, fadeInDuration, fadeOutDuration) { }

    protected override void ConfigureUIElementsOnAwake()
    {
        _closeButton = GetButtonAndRegisterCallback("CloseButton", OnCloseClicked);
    }

    void OnCloseClicked(ClickEvent evt)
    {
        base.ExitState();
        _soundManager.PlayButtonClickSFX();

        if (_gameManager.IsInMainMenuState)
            _uiManager.SwitchToMainMenuState();
        else if (_gameManager.IsInPauseState)
            _uiManager.SwitchToPauseState();
    }
}