using UnityEngine;

public class Shopping_VillagerStrategy : ATimedStrategy
{
    Stall _marketStall;

    public Shopping_VillagerStrategy(INPC npc, float min = 30, float max = 120)
    : base(npc, min, max)
    { }

    public override Node.Status Start()
    {
        _marketStall = ((Villager)_npc).MarketStall;

        // TODO uncomment when generic AStrategy is available
        // if (_marketStall == null)
        // {
        //     if (_npc.DebugMode)
        //         Debug.Log($"[Shopping_VillagerStrategy.Start()] {_npc.Name} has no assigned stall to shop from. Ending shopping.");

        //     return Node.Status.Failure;
        // }

        _npc.AnimationController.ChangeToTalk();
        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // TODO uncomment when generic AStrategy is available
        // // No assigned stall
        // if (_marketStall == null)
        // {
        //     if (_npc.DebugMode)
        //         Debug.Log($"[Shopping_VillagerStrategy.Update()] {_npc.Name} has no assigned stall to shop from. Ending shopping.");

        //     return Node.Status.Failure;
        // }

        // // Stall is closed
        // if (!_marketStall.IsWorkplaceOpen)
        // {
        //     if (_npc.DebugMode)
        //         Debug.Log($"[Shopping_VillagerStrategy.Update()] {_npc.Name} found that stall {_marketStall.name} is closed. Ending shopping.");

        //     return Node.Status.Failure;
        // }

        return base.Update();
    }
}
