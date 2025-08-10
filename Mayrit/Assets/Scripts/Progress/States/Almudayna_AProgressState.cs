using UnityEngine;

public class Almudayna_AProgressState : AProgressState
{
    public Almudayna_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Almudayna", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}