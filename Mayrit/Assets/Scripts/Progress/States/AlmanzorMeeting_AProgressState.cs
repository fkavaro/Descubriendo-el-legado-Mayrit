using UnityEngine;

public class AlmanzorMeeting_AProgressState : AProgressState
{
    public AlmanzorMeeting_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine stateMachine)
    : base("Almanzor meeting", milestone, milestoneInfoSO, stateMachine) { }
}
