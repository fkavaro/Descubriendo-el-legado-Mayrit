using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : ABehaviourEntity<FiniteStateMachine<AGameState>>
{
    #region PROPERTY HELPERS
    public GameInputActions InputActions => _inputActions;
    public bool IsInMainMenuState => _fsm.IsCurrentState(_mainMenuState);
    public bool IsInGamePlayState => _fsm.IsCurrentState(_gamePlayState);
    public bool IsInPauseState => _fsm.IsCurrentState(_pauseState);
    #endregion

    #region INTERNAL PROPERTIES
    GameInputActions _inputActions;
    FiniteStateMachine<AGameState> _fsm;
    MainMenu_GameState _mainMenuState;
    GamePlay_GameState _gamePlayState;
    Pause_GameState _pauseState;
    #endregion

    #region INHERITED
    public override FiniteStateMachine<AGameState> DefineBehaviourSystemOnAwake()
    {
        _fsm = new(this);

        // States initialization
        _mainMenuState = new();
        _gamePlayState = new();
        _pauseState = new();

        // State AwakeState calls
        _mainMenuState.AwakeState();
        _gamePlayState.AwakeState();
        _pauseState.AwakeState();

        _fsm.SetInitialState(_mainMenuState);

        return _fsm;
    }
    #endregion

    #region LIFE CYCLE
    protected override void Awake()
    {
        ServiceLocator.Instance.Register(this);
        _inputActions = new();

        base.Awake();
    }

    void OnDisable()
    {
        ServiceLocator.Instance.Unregister(this);
    }

    void OnDestroy()
    {
        _inputActions = null;
    }
    #endregion

    #region STATES HANDLERS
    public void SwitchToMainMenuState() => _fsm?.SwitchState(_mainMenuState);
    public void SwitchToGamePlayState() => _fsm?.SwitchState(_gamePlayState);
    public void SwitchToPauseState() => _fsm?.SwitchState(_pauseState);
    #endregion
}
