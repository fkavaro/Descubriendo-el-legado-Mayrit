using System;
using UnityEngine;

public class Vision_AProgressState : AProgressState
{
    public Vision_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine stateMachine)
    : base("Vision", milestone, milestoneInfoSO, stateMachine) { }
}