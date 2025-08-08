using System;
using UnityEngine;

public class Albacar_AProgressState : AProgressState
{
    public Albacar_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Albacar", milestone, milestoneInfoSO, stateMachine, nextState) { }

    public override void UpdateState()
    {

    }
}
