using System;
using UnityEngine;

public class Albacar_AProgressState : AProgressState
{
    public Albacar_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Albacar", milestone, milestoneInfoSO, stateMachine) { }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }
}
