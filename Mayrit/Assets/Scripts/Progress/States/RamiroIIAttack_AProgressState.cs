using UnityEngine;

public class RamiroIIAttack_AProgressState : AProgressState
{
    public RamiroIIAttack_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    StackFiniteStateMachine<ProgressManager> stateMachine,
    AProgressState nextState = null)
    : base("Ramiro II attack", milestone, milestoneInfoSO, stateMachine, nextState) { }

    public override void UpdateState()
    {

    }
}
