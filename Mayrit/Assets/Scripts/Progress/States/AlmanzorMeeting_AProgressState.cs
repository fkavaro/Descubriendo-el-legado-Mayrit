using UnityEngine;

public class AlmanzorMeeting_AProgressState : AProgressState
{
    public AlmanzorMeeting_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Almanzor meeting", milestone, milestoneInfoSO, stateMachine, nextState) { }

    public override void UpdateState()
    {

    }
}
