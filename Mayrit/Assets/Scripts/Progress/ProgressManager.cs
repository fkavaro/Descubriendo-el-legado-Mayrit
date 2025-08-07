using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressManager : Singleton<ProgressManager>
{
    public enum Milestone
    {
        Vision,
        Foundation,
        Albacar,
        Almudayna,
        RamiroII,
        Almanzor,
        School,
        Conquest,
    }

    // [Serializable]
    // public struct MilestoneEntry
    // {
    //     public Milestone milestone;
    //     public MilestoneInformationSO informationSO;
    // }


    #region PUBLIC PROPERTIES
    public event Action<Milestone> OnMilestoneChanged;

    [Header("Milestone properties")]
    public Milestone _currentMilestone;

    [Space(10)]
    public MilestoneInformationSO _visionInformation;
    public MilestoneInformationSO _foundationInformation;
    public MilestoneInformationSO _albacarInformation;
    public MilestoneInformationSO _almudaynaInformation;
    public MilestoneInformationSO _ramiroAttackInformation;
    public MilestoneInformationSO _almanzorInformation;
    public MilestoneInformationSO _schoolInformation;
    public MilestoneInformationSO _conquestInformation;

    // State Machine
    public StackFiniteStateMachine<ProgressManager> _fsm;
    public Vision_AProgressState _visionState;
    public Albacar_AProgressState _albacarState;
    public Almudayna_AProgressState _almudaynaState;
    public RamiroIIAttack_AProgressState _ramiroIIState;
    public AlmanzorMeeting_AProgressState _almanzorState;
    public MaslamaSchool_AProgressState _schoolState;
    public Foundation_AProgressState _foundationState;
    public Conquest_AProgressState _conquestState;
    #endregion

    #region PRIVATE PROPERTIES

    #endregion

    #region INHERITED
    protected override void OnAwake()
    {
        // Singleton
        base.OnAwake();
    }

    protected override void OnStart()
    {
        //_currentMilestoneId = 0;
        //_currentMilestone = _milestones[_currentMilestoneId];

        // Notify listeners about the initial milestone
        OnMilestoneChanged?.Invoke(_currentMilestone);

        // Set current playable character
        GameManager.Instance.GetCurrentPlayableCharacter();
    }

    protected override void OnUpdate()
    {

    }

    protected override ADecisionSystem<ProgressManager> CreateDecisionSystem()
    {
        _fsm = new(this);

        // States initialization (from last to first)
        _conquestState = new(Milestone.Conquest, _conquestInformation, _fsm);
        _schoolState = new(Milestone.School, _schoolInformation, _fsm, _conquestState);
        _almanzorState = new(Milestone.Almanzor, _almanzorInformation, _fsm, _schoolState);
        _ramiroIIState = new(Milestone.RamiroII, _ramiroAttackInformation, _fsm, _almanzorState);
        _almudaynaState = new(Milestone.Almudayna, _almudaynaInformation, _fsm, _ramiroIIState);
        _albacarState = new(Milestone.Albacar, _albacarInformation, _fsm, _almudaynaState);
        _foundationState = new(Milestone.Foundation, _foundationInformation, _fsm, _albacarState);
        _visionState = new(Milestone.Vision, _visionInformation, _fsm, _foundationState);

        _fsm.SetInitialState(_visionState);

        return _fsm;
    }
    #endregion

    #region PUBLIC METHODS
    public void SwitchToNextMilestone()
    {
        if (_fsm.SwitchToNextState())
        {
            //_currentMilestoneId++;
            //ChangeMilestone();
        }
    }

    public void SwitchToPreviousMilestone()
    {
        if (_fsm.SwitchToPreviousState())
        {
            //_currentMilestoneId--;
            //ChangeMilestone();
        }
    }

    public bool AtLastMilestone()
    {
        return _currentMilestone.Equals(Milestone.Conquest);
    }

    public bool AtFirstMilestone()
    {
        return _currentMilestone.Equals(Milestone.Vision);
    }

    public void InvokeOnMilestoneChanged()
    {
        OnMilestoneChanged?.Invoke(_currentMilestone);
    }
    #endregion

    #region PRIVATE METHODS
    // void ChangeMilestone()
    // {
    //     //_currentMilestone = _milestones[_currentMilestoneId];
    //     //_currentMilestone = _fsm.CurrentState._milestone;
    //     //OnMilestoneChanged?.Invoke(_currentMilestone.milestone);

    //     // Update current playable character
    //     GameManager.Instance.GetCurrentPlayableCharacter();
    // }
    #endregion
}
