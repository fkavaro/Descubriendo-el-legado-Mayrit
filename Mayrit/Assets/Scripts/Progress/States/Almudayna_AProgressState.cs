using UnityEngine;

public class Almudayna_AProgressState : AProgressState
{
    public Almudayna_AProgressState(ProgressManager.Milestone milestone,
    Milestone_InformationSO milestoneInfoSO,
    FiniteStateMachine<ProgressManager> stateMachine)
    : base("Almudayna", milestone, milestoneInfoSO, stateMachine) { }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }
}