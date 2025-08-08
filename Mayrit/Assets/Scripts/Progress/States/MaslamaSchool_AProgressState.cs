using UnityEngine;

public class MaslamaSchool_AProgressState : AProgressState
{
    public MaslamaSchool_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Mathematics and Astronomy school", milestone, milestoneInfoSO, stateMachine, nextState) { }
    public override void UpdateState()
    {

    }
}