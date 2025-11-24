using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the progress states and data. Singleton.
/// </summary>
public class ProgressManager : ASingletonBehaviourEntity<ProgressManager, FiniteStateMachine<AProgressState>>
{
    #region PROPERTY HELPERS
    public int CurrentMilestoneIndex => _currentMilestoneIndex;
    #endregion

    #region EDITOR PROPERTIES
    [Header("Milestones")]
    [SerializeField] bool _updateInInspector = true;
    [SerializeField] int _currentMilestoneIndex = 0;
    [SerializeField] List<Milestone_InformationSO> _milestoneSequence;
    #endregion

    #region INTERNAL PROPERTIES
    public event Action<int> OnMilestoneChangedEvent;
    public event Action<bool> OnEditorUpdateChangedEvent;
    public event Action<float> OnTimeSetEvent;

    int _lastValidatedMilestoneIndex;
    bool _lastUpdateInInspector;

    FiniteStateMachine<AProgressState> _fsm;
    #endregion

    #region INHERITED
    public override FiniteStateMachine<AProgressState> InitializeBehaviourSystem()
    {
        _fsm = new(this);

        // Susbcribe to state switch event to update current milestone
        _fsm.OnStateSwitchEvent += OnStateSwitch;

        // States initialization
        Vision_AProgressState _visionState = new(_milestoneSequence[0]);
        Foundation_AProgressState _foundationState = new(_milestoneSequence[1]);
        Albacar_AProgressState _albacarState = new(_milestoneSequence[2]);
        Almudayna_AProgressState _almudaynaState = new(_milestoneSequence[3]);
        RamiroIIAttack_AProgressState _ramiroIIState = new(_milestoneSequence[4]);
        AlmanzorMeeting_AProgressState _almanzorState = new(_milestoneSequence[5]);
        MaslamaSchool_AProgressState _schoolState = new(_milestoneSequence[6]);
        Conquest_AProgressState _conquestState = new(_milestoneSequence[7]);

        // Add states to the FSM sequence
        _fsm.AddStateToSequence(_visionState);
        _fsm.AddStateToSequence(_foundationState);
        _fsm.AddStateToSequence(_albacarState);
        _fsm.AddStateToSequence(_almudaynaState);
        _fsm.AddStateToSequence(_ramiroIIState);
        _fsm.AddStateToSequence(_almanzorState);
        _fsm.AddStateToSequence(_schoolState);
        _fsm.AddStateToSequence(_conquestState);

        _fsm.SetInitialState(_visionState);

        return _fsm;
    }
    #endregion

    // Called when the script is loaded or a value is changed in the inspector
    void OnValidate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            return;

        // Invoke milestone changed event when _currentMilestone is changed in inspector
        // and _updateInInspector is true
        if (_lastValidatedMilestoneIndex != _currentMilestoneIndex)
        {
            _lastValidatedMilestoneIndex = _currentMilestoneIndex;

            if (_updateInInspector)
            {
                // To avoid issues with re-entrancy
                UnityEditor.EditorApplication.delayCall += () => OnMilestoneChangedEvent?.Invoke(_currentMilestoneIndex);
            }
        }

        // Invoke editor update changed event when _updateInInspector is changed in inspector
        if (_lastUpdateInInspector != _updateInInspector)
        {
            _lastUpdateInInspector = _updateInInspector;
            bool update = _updateInInspector;

            UnityEditor.EditorApplication.delayCall += () => OnEditorUpdateChangedEvent?.Invoke(update);
        }
#endif
    }

    #region PUBLIC METHODS
    public void SwitchToNextMilestone()
    {
        _fsm.SwitchToNextStateInSequence();
    }

    public void SwitchToPreviousMilestone()
    {
        _fsm.SwitchToPreviousStateInSequence();
    }

    public bool AtFirstMilestone()
    {
        return _fsm.AtFistStateInSequence();
    }

    public bool AtLastMilestone()
    {
        return _fsm.AtLastStateInSequence();
    }

    public Milestone_InformationSO GetCurrentMilestoneInfo()
    {
        return _milestoneSequence[_currentMilestoneIndex];
    }
    #endregion

    #region EVENT METHODS
    void OnStateSwitch()
    {
        _currentMilestoneIndex = _fsm.CurrentState._milestoneInformation.MilestoneIndex;
        OnMilestoneChangedEvent?.Invoke(_currentMilestoneIndex);
        OnTimeSetEvent?.Invoke(_fsm.CurrentState._milestoneInformation.WantedTime);
    }
    #endregion
}
