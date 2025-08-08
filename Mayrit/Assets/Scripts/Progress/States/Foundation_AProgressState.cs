using System;
using UnityEngine;

public class Foundation_AProgressState : AProgressState
{
    public Foundation_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Foundation", milestone, milestoneInfoSO, stateMachine, nextState) { }

    public override void UpdateState()
    {

    }
}
