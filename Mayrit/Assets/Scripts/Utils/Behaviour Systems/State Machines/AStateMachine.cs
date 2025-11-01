using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for a state machine that handles the states of a controller.
/// </summary>
public abstract class AStateMachine<TStateMachineType> : ABehaviourSystem
where TStateMachineType : AStateMachine<TStateMachineType>
{
    #region PROPERTIES
    public AState<TStateMachineType> CurrentState => _currentState;
    public string _currentStateName = "None";
    protected AState<TStateMachineType> _currentState, _initialState;
    protected List<AState<TStateMachineType>> _statesSequence = new();
    #endregion

    #region CONSTRUCTOR
    protected AStateMachine(IBehaviourEntity entity)
    : base(entity) { }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    public abstract void SwitchState(AState<TStateMachineType> state);
    #endregion

    #region INHERITED METHODS
    /// <summary>
    /// Switchs back to initial state
    /// </summary>
    public override void Reset()
    {
        SwitchState(_initialState);
    }

    /// <summary>
    /// Debugs the current state of the state machine.
    /// </summary>
    protected override void DebugDecision()
    {
        _currentStateName = _currentState.StateName;

        if (_behaviourEntity.DebugMode)
            Debug.Log("[" + _behaviourEntity.GO.name + "]" + " is " + _currentState.StateName);
    }
    #endregion

    #region PUBLIC METHODS
    public virtual void SetInitialState(AState<TStateMachineType> state)
    {
        if (state == _currentState) return;

        _initialState = state;
    }

    public bool IsCurrentState(AState<TStateMachineType> state)
    {
        return _currentState == state;
    }

    public virtual void ForceState(AState<TStateMachineType> newState)
    {
        if (newState == _currentState) return;

        // Don't exit the current state, just set and start the new one
        _currentState = newState;
        DebugDecision();
        _currentState.StartState();
    }

    public void AddStateToSequence(AState<TStateMachineType> state)
    {
        if (_statesSequence.Contains(state)) return;
        _statesSequence.Add(state);
        _initialState ??= state; // Set as initial state if none set
    }

    /// <summary>
    /// Switches to the previous state in the list.
    /// </summary>
    public bool SwitchToPreviousStateInSequence()
    {
        if (_statesSequence.Count == 0 || _currentState == null) return false;

        int currentIndex = _statesSequence.IndexOf(_currentState);

        if (currentIndex <= 0) // First state
            return false;

        AState<TStateMachineType> previousState = _statesSequence[currentIndex - 1];

        SwitchState(previousState);
        return true;
    }

    /// <summary>
    /// Switches to the next state in the list.
    /// </summary>
    public virtual bool SwitchToNextStateInSequence()
    {
        if (_statesSequence.Count == 0 || _currentState == null) return false;

        int currentIndex = _statesSequence.IndexOf(_currentState);

        if (currentIndex >= _statesSequence.Count - 1) // Last state
            return false;

        AState<TStateMachineType> nextState = _statesSequence[currentIndex + 1];

        SwitchState(nextState);
        return true;
    }
    #endregion

    #region MONOBEHAVIOUR EQUIVALENTS: DERIVED TO CURRENT STATE
    public override void Awake()
    {
        if (_initialState == null)
        {
            Debug.LogWarning(_behaviourEntity.GO.name + ": AStateMachine has no initial state set.");
            return;
        }

        _currentState = _initialState;
        DebugDecision();
        _currentState?.AwakeState();
    }

    public override void Start()
    {
        _currentState?.StartState();
    }

    public override void Update()
    {
        if (!_behaviourEntity.IsExecutionPaused)
            _currentState?.OnUpdateState();
    }

    public override void LateUpdate()
    {
        if (!_behaviourEntity.IsExecutionPaused)
            _currentState?.OnLateUpdateState();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        _currentState?.OnCollisionEnter(collision);
    }

    public override void OnCollisionStay(Collision collision)
    {
        _currentState?.OnCollisionStay(collision);
    }

    public override void OnCollisionExit(Collision collision)
    {
        _currentState?.OnCollisionExit(collision);
    }

    public override void OnTriggerEnter(Collider other)
    {
        _currentState?.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        _currentState?.OnTriggerStay(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        _currentState?.OnTriggerExit(other);
    }
    #endregion
}
