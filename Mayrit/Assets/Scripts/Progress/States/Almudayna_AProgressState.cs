using UnityEngine;

public class Almudayna_AProgressState : AProgressState
{
    public Almudayna_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Almudayna", milestone, milestoneInfoSO, stateMachine, nextState) { }

    public override void UpdateState()
    {

    }
}