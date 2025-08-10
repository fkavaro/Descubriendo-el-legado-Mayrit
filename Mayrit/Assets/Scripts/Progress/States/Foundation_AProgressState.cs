using System;
using UnityEngine;

public class Foundation_AProgressState : AProgressState
{
    public Foundation_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Foundation", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}
