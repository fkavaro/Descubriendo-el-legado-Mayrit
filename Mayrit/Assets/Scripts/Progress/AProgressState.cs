using System;
using UnityEngine;

public abstract class AProgressState : AState<ProgressManager, StackFiniteStateMachine<ProgressManager>>
{
    public readonly MilestoneInformationSO _informationSO;
    public readonly ProgressManager.Milestone _milestone;

    public AProgressState(string name,
        ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSo,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base(name, stateMachine, nextState)
    {
        _milestone = milestone;
        _informationSO = milestoneInfoSo;
    }

    public override void StartState()
    {
        ProgressManager.Instance._currentMilestone = _milestone;
        ProgressManager.Instance.InvokeOnMilestoneChanged();

        // Update current playable character
        GameManager.Instance.GetCurrentPlayableCharacter();
    }
}