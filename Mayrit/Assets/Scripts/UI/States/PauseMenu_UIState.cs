using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PauseMenu_UIState : AUIState
{
    #region PUBLIC PROPERTIES
    #endregion

    #region PRIVATE PROPERTIES
    Button _playButton;
    #endregion

    #region INHERITED
    public PauseMenu_UIState(FiniteStateMachine<UIManager> stateMachine)
    : base("PauseMenu", stateMachine) { }

    public override void AwakeState()
    {
        _UIDocument = UIManager.Instance.UIDocument;
        _screen = _UIDocument.rootVisualElement.Q<VisualElement>("PauseMenu");
        _playButton = _screen.Q<Button>("PlayButton");

        _playButton.RegisterCallback<ClickEvent>(SwitchToHUDState);
    }

    public override void StartState()
    {
        _screen.style.display = DisplayStyle.Flex; // Show pause menu

        // Game pause state
        GameManager.Instance.fsm.SwitchState(GameManager.Instance.pauseState);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _screen.style.display = DisplayStyle.None; // Hide HUD
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS
    void SwitchToHUDState(ClickEvent evt)
    {
        _stateMachine.SwitchState(UIManager.Instance.hudState); // Switch to HUD state
        GameManager.Instance.fsm.SwitchState(GameManager.Instance.gamePlayState);
    }
    #endregion
}
