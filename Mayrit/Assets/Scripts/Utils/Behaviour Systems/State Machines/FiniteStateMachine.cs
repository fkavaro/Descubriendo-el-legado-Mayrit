using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finite State Machine implementation for controlling a behaviour.
/// </summary>
public class FiniteStateMachine<StateType> : AStateMachine<StateType>
where StateType : AState
{
    #region CONSTRUCTOR
    public FiniteStateMachine(IBehaviourEntity entity)
    : base(entity) { }
    #endregion

    #region INHERITED METHODS
    /// <summary>
    /// Switchs to another state after exiting the current.
    /// </summary>
    public override void SwitchState(StateType newState)
    {
        if (newState == _currentState)
        {
            if (_behaviourEntity.DebugMode)
                Debug.LogWarning($"{_behaviourEntity.GO.name} tried to switch to the same state: {newState?.StateName}");
            return;
        }

        // if (_behaviourEntity.DebugMode)
        //     Debug.Log($"{_behaviourEntity.GO.name} switching state from {_currentState?.StateName} to {newState?.StateName}");

        _currentState?.ExitState();
        _currentState = newState;
        DebugDecision();
        _currentState?.StartState();

        // Invoke switch event
        base.SwitchState(newState);
    }
    #endregion
}
