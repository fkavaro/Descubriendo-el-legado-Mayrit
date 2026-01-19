using UnityEngine;

public class GoToMarket_VillagerStrategy : ANPCStrategy<Villager>
{
    readonly Market _market;
    Spot _marketStallSpot;
    bool _isWaitingForAccess;

    const float STALL_CHECK_DISTANCE = 5f;

    public GoToMarket_VillagerStrategy(Villager npc, Market market)
    : base(npc)
    {
        _market = market;
    }

    public override Node.Status Start()
    {
        CleanupStaleConversation();

        if (!TrySetStallDestination(preferOpen: false))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] can't go to market.", _npc.GO);
            return Node.Status.Failure;
        }

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        if (!ValidateMarketState())
            return Node.Status.Failure;

        if (!EnsureCorrectDestination())
            return Node.Status.Failure;

        if (ShouldSwitchStall())
        {
            if (!TrySetStallDestination(preferOpen: true))
            {
                if (_npc.DebugMode)
                    Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] no available stalls found", _npc.GO);
                return Node.Status.Failure;
            }
            return Node.Status.Running;
        }

        if (!_npc.Market.IsOpen())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] market is closed", _npc.GO);
            return Node.Status.Failure;
        }

        if (HasArrivedAtStall())
        {
            _npc.MarketStall.UnregisterClientWaiting(_npc);
            return Node.Status.Success;
        }

        return HandleApproachingStall();
    }

    void CleanupStaleConversation()
    {
        if (_npc.InteractionController.IsTalking())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] starting routine with stale conversation state - cleaning up", _npc.GO);
            _npc.InteractionController.ConversationInterrupted();
        }
    }

    bool ValidateMarketState()
    {
        if (_market == null || _npc.MarketStall == null || _marketStallSpot == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] invalid market state", _npc.GO);
            return false;
        }
        return true;
    }

    bool EnsureCorrectDestination()
    {
        if (!_npc.MovementController.IsDestinationSpot(_marketStallSpot))
            return _npc.MovementController.SetDestinationSpot(_marketStallSpot);
        return true;
    }

    bool ShouldSwitchStall()
    {
        if (!_npc.MovementController.IsCloseToPosition(_marketStallSpot.transform.position, STALL_CHECK_DISTANCE))
            return false;

        return !_npc.MarketStall.IsOpen || _npc.MarketStall.TooManyClientsWaiting;
    }

    bool HasArrivedAtStall()
    {
        return _npc.MovementController.HasArrivedAtSpot(_marketStallSpot, true);
    }

    Node.Status HandleApproachingStall()
    {
        // Still far from stall
        if (!_npc.MovementController.IsCloseToSpot(_marketStallSpot))
        {
            _npc.MovementController.SetIfStopped(false);
            return Node.Status.Running;
        }

        // Close to stall but spot is occupied
        if (_marketStallSpot.IsOccupied())
        {
            HandleWaitingForSpot();
            return Node.Status.Running;
        }

        // Close to stall and spot is free - resume movement
        ResumeMovementToSpot();
        return Node.Status.Running;
    }

    void HandleWaitingForSpot()
    {
        if (!_isWaitingForAccess)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] stall spot occupied, waiting", _npc.GO);

            _npc.MovementController.SetIfStopped(true);
            _npc.AnimationController.ChangeToIdle();
            _npc.MarketStall.RegisterClientWaiting(_npc);
            _isWaitingForAccess = true;
        }

        _npc.MovementController.RotateSmoothlyTowards(_npc.MarketStall.gameObject);
    }

    void ResumeMovementToSpot()
    {
        _isWaitingForAccess = false;
        _npc.MovementController.SetIfStopped(false);
        _npc.AnimationController.ChangeToWalk();
    }

    bool TrySetStallDestination(bool preferOpen)
    {
        // Try to get preferred stall type
        _npc.MarketStall = preferOpen ? _market.GetRandomOpenedStall() : _market.GetRandomStall();

        // Fallback to any stall if preferred type not available
        if (_npc.MarketStall == null && preferOpen)
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
