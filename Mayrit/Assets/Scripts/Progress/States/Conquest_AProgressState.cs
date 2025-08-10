using System;
using UnityEngine;

public class Conquest_AProgressState : AProgressState
{
    public Conquest_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Conquest", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}
