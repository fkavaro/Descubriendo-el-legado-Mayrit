using UnityEngine;

public class Working_VillagerStrategy : ATimedStrategy
{
    readonly Workplace _workplace;

    public Working_VillagerStrategy(Villager villager, Workplace workplace, float min = 30, float max = 120)
    : base(villager, min, max)
    {
        _workplace = workplace;
    }

    // Start
    public override Node.Status Start()
    {
        _npc.AnimationController.ChangeToIdle();
        _workplace.IsWorkplaceOpen = true;

        return Node.Status.Success;
    }

    public override void OnTimerComplete()
    {
        _workplace.IsWorkplaceOpen = false;
    }
}
