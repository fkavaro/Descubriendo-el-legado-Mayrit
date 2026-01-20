using UnityEngine;

public class GoToMarket_VillagerStrategy : ANPCStrategy<Villager>
{
    readonly Market _market;
    Spot _marketStallSpot;
    bool _isWaitingForAccess;

    const float STALL_CHECK_DISTANCE = 15f;

    public GoToMarket_VillagerStrategy(Villager npc, Market market)
    : base(npc)
    {
        _market = market;
    }

    public override Node.Status Start()
    {
        CleanupStaleConversation();

        if (!TrySetStallDestination(onlyOpen: false))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] can't go to market.", _npc.GO);
            return Node.Status.Failure;
        }

        // if (_npc.DebugMode)
        //     Debug.Log($"[{_npc.Name}.GoToMarket_VillagerStrategy.Start()] going to market stall.", _npc.GO);

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        if (!ValidateMarketState())
            return Node.Status.Failure;

        if (!EnsureCorrectDestination())
            return Node.Status.Failure;

        if (ShouldSwitchStallWhenClose())
        {
            // No open stall found: failure
            if (!TrySetStallDestination(onlyOpen: true))
            {
                if (_npc.DebugMode)
                    Debug.LogWarning($"[{_npc.Name}.GoToMarket_VillagerStrategy.Update()] no open stalls available", _npc.GO);
                return Node.Status.Failure;
            }
            return Node.Status.Running;
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
            return _npc.MovementController.TrySetDestinationSpot(_marketStallSpot);
        return true;
    }

    bool ShouldSwitchStallWhenClose()
    {
        if (!_npc.MovementController.IsFarFromPosition(_marketStallSpot.transform.position))
            return false;

        return !_npc.MarketStall.IsOpen || _npc.MarketStall.TooManyClientsWaiting;
    }

    bool HasArrivedAtStall()
    {
        return _npc.MovementController.HasArrivedAtDestinationSpot(_marketStallSpot, true);
    }

    Node.Status HandleApproachingStall()
    {
        // Still far from stall
        if (!_npc.MovementController.IsNearDestinationSpot(_marketStallSpot))
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

    bool TrySetStallDestination(bool onlyOpen = false)
    {
        // Fist try to get an open stall
        Stall newStall = _market.TryGetRandomStall(preferOpen: true, excludedStall: _npc.MarketStall);

        // If allowed, try to find any stall if no open stall found
        if (newStall == null && !onlyOpen)
            newStall = _market.TryGetRandomStall(preferOpen: false, excludedStall: _npc.MarketStall);

        if (newStall == null)
            return false;

        Spot newDestination = newStall.GetRandomAccessSpot();
        if (newDestination == null)
            return false;

        if (!_npc.MovementController.TrySetDestinationSpot(newDestination))
            return false;

        _npc.MarketStall = newStall;
        _marketStallSpot = newDestination;
        _isWaitingForAccess = false;
        return true;
    }
}
