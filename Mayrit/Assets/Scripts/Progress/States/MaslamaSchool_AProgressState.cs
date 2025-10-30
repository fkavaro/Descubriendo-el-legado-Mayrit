using UnityEngine;

public class MaslamaSchool_AProgressState : AProgressState
{
    public MaslamaSchool_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine stateMachine)
    : base("Mathematics and Astronomy school", milestone, milestoneInfoSO, stateMachine) { }
}