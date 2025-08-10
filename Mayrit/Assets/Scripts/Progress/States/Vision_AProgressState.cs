using System;
using UnityEngine;

public class Vision_AProgressState : AProgressState
{
    public Vision_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Vision", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}