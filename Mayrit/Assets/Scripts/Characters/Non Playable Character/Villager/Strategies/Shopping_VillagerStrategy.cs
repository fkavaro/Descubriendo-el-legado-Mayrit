using UnityEngine;

public class Shopping_VillagerStrategy : ATimedNPCStrategy<Villager>
{
    public Shopping_VillagerStrategy(Villager npc, float min = 30, float max = 120)
    : base(npc, min, max)
    { }

    public override Node.Status Start()
    {
        if (_npc.MarketStall == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[Shopping_VillagerStrategy.Start()] {_npc.Name} has no assigned stall to shop from. Ending shopping.");

            return Node.Status.Failure;
        }

        _npc.AnimationController.ChangeToTalk();
        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // No assigned stall
        if (_npc.MarketStall == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[Shopping_VillagerStrategy.Update()] {_npc.Name} has no assigned stall to shop from. Ending shopping.");

            return Node.Status.Failure;
        }

        // Stall is closed
        if (!_npc.MarketStall.IsWorkplaceOpen)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[Shopping_VillagerStrategy.Update()] {_npc.Name} found that stall {_npc.MarketStall.name} is closed. Ending shopping.");

            _npc.MarketStall = null;

            return Node.Status.Failure;
        }

        return base.Update();
    }

    public override void OnTimerComplete()
    {
        // Finished shopping, clear shopping stall
        _npc.MarketStall = null;
    }
}
