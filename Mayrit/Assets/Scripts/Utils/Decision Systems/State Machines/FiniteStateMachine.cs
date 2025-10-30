using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finite State Machine implementation for controlling a behaviour.
/// </summary>
public class FiniteStateMachine : AStateMachine<FiniteStateMachine>
{
    #region CONSTRUCTOR
    public FiniteStateMachine(IBehaviourEntity<ABehaviourSystem> entity, GameObject entityGO)
    : base(entity, entityGO) { }
    #endregion

    #region INHERITED METHODS
    /// <summary>
    /// Switchs to another state after exiting the current.
    /// </summary>
    public override void SwitchState(AState<FiniteStateMachine> state)
    {
        if (state == _currentState) return;

        _currentState?.OnExitState();
        _currentState = state;
        DebugDecision();
        _currentState?.StartState();
    }
    #endregion
}
