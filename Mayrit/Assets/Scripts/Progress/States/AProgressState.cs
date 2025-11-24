using System;
using UnityEngine;

public abstract class AProgressState : AState<FiniteStateMachine>
{
    public readonly ProgressManager.Milestone _milestone;
    public readonly Milestone_InformationSO _informationSO;

    public AProgressState(string name,
        ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSo,
    FiniteStateMachine stateMachine)
    : base(name, stateMachine)
    {
        _milestone = milestone;
        _informationSO = milestoneInfoSo;
    }
}