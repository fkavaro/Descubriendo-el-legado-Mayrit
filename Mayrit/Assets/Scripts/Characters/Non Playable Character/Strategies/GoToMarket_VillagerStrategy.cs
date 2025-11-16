using UnityEngine;

public class GoToMarket_VillagerStrategy : AStrategy
{
    readonly Market _market;
    Spot _marketStallSpot;

    public GoToMarket_VillagerStrategy(INPC npc, Market market)
    : base(npc)
    {
        _market = market;
    }

    public override Node.Status Start()
    {
        _marketStallSpot = _market.GetOpenStallSpot();

        if (_marketStallSpot == null)
            return Node.Status.Failure;

        _npc.SetDestinationSpot(_marketStallSpot);

        if (_npc.IsDestination(_marketStallSpot))
            return Node.Status.Success;
        else
            return Node.Status.Failure;
    }

    public override Node.Status Update()
    {
        // Success if arrived at market
        if (_npc.HasArrivedAt(_marketStallSpot, true, false))
        {
            _npc.AnimationController.ChangeToIdle();
            return Node.Status.Success;
        }
        // Continue if not
        else
        {
            // TODO: check if this work
            // Stop and idle if its near and destination spot is occupied
            if (_npc.IsCloseTo(_marketStallSpot, 2f, true))
            {
                if (_marketStallSpot.IsOccupied())
                {
                    Debug.Log($"{_npc.Name} is near market stall spot but it's occupied. Stopping.");
                    _npc.SetIfStopped(true);
                }
                else
                {
                    _npc.SetIfStopped(false);
                }
            }

            return Node.Status.Running;
        }
    }
}
