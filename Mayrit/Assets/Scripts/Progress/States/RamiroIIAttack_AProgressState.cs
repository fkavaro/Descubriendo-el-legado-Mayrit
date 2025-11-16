using UnityEngine;

public class RamiroIIAttack_AProgressState : AProgressState
{
    public RamiroIIAttack_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine stateMachine)
    : base("Ramiro II attack", milestone, milestoneInfoSO, stateMachine) { }
}
