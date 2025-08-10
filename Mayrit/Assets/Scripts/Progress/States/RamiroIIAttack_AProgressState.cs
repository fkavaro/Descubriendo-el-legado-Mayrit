using UnityEngine;

public class RamiroIIAttack_AProgressState : AProgressState
{
    public RamiroIIAttack_AProgressState(ProgressManager.Milestone milestone,
    MilestoneInformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Ramiro II attack", milestone, milestoneInfoSO, stateMachine) { }

    public override void UpdateState()
    {

    }
}
