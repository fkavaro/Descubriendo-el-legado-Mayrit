using UnityEngine;

public class MaslamaSchool_AProgressState : AProgressState
{
    public MaslamaSchool_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Mathematics and Astronomy school", milestone, milestoneInfoSO, stateMachine) { }
    public override void UpdateState()
    {

    }
}