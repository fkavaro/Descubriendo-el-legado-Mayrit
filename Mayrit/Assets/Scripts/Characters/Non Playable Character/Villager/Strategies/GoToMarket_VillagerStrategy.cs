using UnityEngine;

public class GoToMarket_VillagerStrategy : ANPCStrategy<Villager>
{
    readonly Market _market;
    Spot _marketStallSpot;
    bool _isWaitingForAccess;

    public GoToMarket_VillagerStrategy(Villager npc, Market market)
    : base(npc)
    {
        _market = market;
    }

    public override Node.Status Start()
    {
        // Clean up any stale conversation state
        if (_npc.InteractionController.IsTalking())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] starting routine with stale conversation state - cleaning up", _npc.GO);
            _npc.InteractionController.ConversationInterrupted();
        }

        if (!GetStallAndSetDestination())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ {_npc.Name}.GoToMarket_VillagerStrategy.Update()] could not find an available stall spot in the market.", _npc.GO);

            return Node.Status.Failure;
        }

        if (_npc.DebugMode)
            Debug.Log($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] heading to market stall spot", _npc.GO);

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Guard: validate market/stall/spot state
        if (_market == null || _npc.MarketStall == null || _marketStallSpot == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] invalid market state", _npc.GO);
            return Node.Status.Failure;
        }

        // Fix destination if needed; fail if unreachable
        if (!_npc.MovementController.IsDestinationSpot(_marketStallSpot))
        {
            if (!_npc.MovementController.SetDestinationSpot(_marketStallSpot))
                return Node.Status.Failure;
        }

        // Not close enough yet - keep moving
        if (!_npc.MovementController.IsCloseToSpot(_marketStallSpot))
        {
            _npc.MovementController.SetIfStopped(false);
            return Node.Status.Running;
        }

        // Close to stall - fail if market is closed
        if (!_market.IsOpen())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] market is closed", _npc.GO);
            return Node.Status.Failure;
        }

        // Close to stall - check if stall is still open
        if (!_npc.MarketStall._isOpen)
        {
            // Stall closed - try another
            if (!GetStallAndSetDestination())
            {
                if (_npc.DebugMode)
                    Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] no available stalls found", _npc.GO);
                return Node.Status.Failure;
            }
            return Node.Status.Running;
        }

        // Success if arrived at stall spot
        if (_npc.MovementController.HasArrivedAtSpot(_marketStallSpot, true))
            return Node.Status.Success;

        // Not arrived yet - check if spot is occupied by another NPC
        if (_marketStallSpot.IsOccupied())
        {
            // Wait for spot to become available
            if (!_isWaitingForAccess)
            {
                if (_npc.DebugMode)
                    Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] stall spot occupied, waiting", _npc.GO);

                _npc.MovementController.SetIfStopped(true);
                _npc.AnimationController.ChangeToIdle();

                _isWaitingForAccess = true;
            }

            _npc.MovementController.RotateSmoothlyTowards(_npc.MarketStall.gameObject);
            return Node.Status.Running;
        }

        // Spot available and we haven't arrived yet - move to it
        _isWaitingForAccess = false;
        _npc.MovementController.SetIfStopped(false);
        _npc.AnimationController.ChangeToWalk();

        return Node.Status.Running;
    }

    bool GetStallAndSetDestination()
    {
        _npc.MarketStall = _market.GetRandomStall();
        if (_npc.MarketStall == null)
            return false;

        _marketStallSpot = _npc.MarketStall.GetRandomAccessSpot();
        if (_marketStallSpot == null)
            return false;

        if (!_npc.MovementController.SetDestinationSpot(_marketStallSpot))
            return false;

        _isWaitingForAccess = false;
        return true;
    }
}
