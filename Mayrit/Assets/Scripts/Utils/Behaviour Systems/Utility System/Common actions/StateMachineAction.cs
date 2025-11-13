using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that runs a finite state machine (FSM) on an Utility System.
/// </summary>
public class StateMachineAction<TStateMachine> : ABinaryAction
where TStateMachine : AStateMachine<TStateMachine>
{
    #region PROPERTIES
    readonly TStateMachine _stateMachine;
    bool _alreadyStarted = false;
    #endregion

    #region CONSTRUCTOR
    public StateMachineAction(UtilitySystem utilitySystem, TStateMachine stateMachine)
    : base("FSM", utilitySystem, 0.5f)
    {
        _stateMachine = stateMachine;
    }
    #endregion

    #region INHERITED METHODS
    protected override bool SetDecisionFactor()
    {
        return true; // Will remain valid action
    }

    public override void StartAction()
    {
        // Start just once - to maintain current state after returning from other action
        if (!_alreadyStarted)
        {
            _alreadyStarted = true;
            _stateMachine.Start();
        }
    }

    public override void UpdateAction()
    {
        _stateMachine.Update();
    }

    public override bool IsFinished()
    {
        return true; // Allows evaluation of other actions
    }

    /// <returns>State name of FSM action</returns>
    public override string DebugAction()
    {
        return _stateMachine.CurrentState?.StateName;
    }

    public override void Reset()
    {
        _alreadyStarted = false;
        _stateMachine.Reset();
    }
    #endregion

    #region PUBLIC METHODS
    public void ForceState(AState<TStateMachine> newState)
    {
        _stateMachine.ForceState(newState);
    }

    public bool IsCurrentState(AState<TStateMachine> state)
    {
        return _stateMachine.IsCurrentState(state);
    }
    #endregion
}