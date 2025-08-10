using UnityEngine;

public class AlmanzorMeeting_AProgressState : AProgressState
{
    public AlmanzorMeeting_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Almanzor meeting", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}
