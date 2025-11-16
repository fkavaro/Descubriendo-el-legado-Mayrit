using UnityEngine;
using System.Collections;

/// <summary>
/// Base class with common functionalities for all states.
/// </summary>
public abstract class AState<TStateMachine>
    where TStateMachine : AStateMachine<TStateMachine>
{
    #region PROPERTIES
    public string StateName => _stateName;
    protected string _stateName;
    protected TStateMachine _stateMachine;
    protected float _stateTime = 0f;
    protected readonly AState<TStateMachine> _nextState;
    #endregion

    #region CONSTRUCTOR
    public AState(string statename,
    TStateMachine stateMachine)
    {
        _stateName = statename;
        _stateMachine = stateMachine;
        _stateMachine.AddStateToSequence(this);
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Checks if this state is the current state in the state machine.
    /// </summary>
    public bool IsCurrentState()
    {
        return _stateMachine.IsCurrentState(this);
    }

    public void OnExitState()
    {
        _stateTime = 0f; // Reset the state time
        ExitState(); // Call the ExitState method implemented in subclasses
    }

    /// <summary>
    /// Called when exiting the state.
    /// </summary>
    public virtual void ExitState() { }

    public virtual void SwitchState(AState<TStateMachine> nextState)
    {
        _stateMachine?.SwitchState(nextState);
    }

    /// <summary>
    /// Coroutine to wait for a random amount of time before switching to the next state.
    /// </summary>
    public IEnumerator SwitchStateAfterRandomTime(AState<TStateMachine> nextState, int min = 5, int max = 21)
    {
        int waitTime = Random.Range(min, max);
        return SwitchStateAfterCertainTime(nextState, waitTime);
    }

    /// <summary>
    /// Coroutine to wait for a specified amount of time before switching to the next state.
    /// </summary>
    public virtual IEnumerator SwitchStateAfterCertainTime(AState<TStateMachine> nextState, float waitTime)
    {
        _stateMachine._behaviourEntity.IsExecutionPaused = true;

        yield return new WaitForSeconds(waitTime);

        _stateMachine?.SwitchState(nextState);
        _stateMachine._behaviourEntity.IsExecutionPaused = false;
    }
    #endregion

    #region MONOBEHAVIOUR EQUIVALENTS
    /// <summary>
    /// Executed in Monobehaviour/Awake() if initial state.
    /// </summary>
    public virtual void AwakeState() { }
    public virtual void StartState() { }

    public void OnUpdateState()
    {
        _stateTime += Time.deltaTime; // Update the state time
        UpdateState(); // Call the UpdateState method implemented in subclasses
    }
    public virtual void UpdateState() { }

    public void OnLateUpdateState()
    {
        LateUpdateState();
    }
    public virtual void LateUpdateState() { }

    public virtual void OnTriggerEnter(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerStay(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerExit(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnCollisionEnter(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionStay(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionExit(Collision collision) { } // Optionally implemented in subclasses
    #endregion
}