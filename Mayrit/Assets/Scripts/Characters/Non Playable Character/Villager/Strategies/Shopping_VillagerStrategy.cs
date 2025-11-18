using UnityEngine;

public class Shopping_VillagerStrategy : ATimedStrategy
{
    public Shopping_VillagerStrategy(INPC npc, float min = 30, float max = 120)
    : base(npc, min, max)
    { }

    // Start
    public override Node.Status Start()
    {
        _npc.AnimationController.ChangeToTalk();
        return Node.Status.Success;
    }
}
