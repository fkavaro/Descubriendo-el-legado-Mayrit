using System;
using UnityEngine;

public class GoToDestinationStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    readonly Func<Spot> _destinationResolver; // Lazy resolver to avoid stale cached spots
    Spot _destinationSpot;
    private readonly bool _fixRotation;

    public GoToDestinationStrategy(NPCtype npc, Func<Spot> destinationResolver, bool fixRotation = false)
    : base(npc)
    {
        _destinationResolver = destinationResolver;
        _fixRotation = fixRotation;
    }

    public override Node.Status Start()
    {
        CleanupStaleConversation();

        _destinationSpot = _destinationResolver?.Invoke();

        if (_destinationSpot == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToDestinationStrategy.Start()] destination spot is null", _npc.GO);
            return Node.Status.Failure;
        }

        if (!_npc.MovementController.TrySetDestinationSpot(_destinationSpot))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[{_npc.Name}.GoToDestinationStrategy.Start()] could not set destination", _npc.GO);
            return Node.Status.Failure;
        }

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        if (!TryEnsureDestination(_destinationSpot))
            return Node.Status.Failure;

        // Success if arrived at destination
        return _npc.MovementController.HasArrivedAtDestinationSpot(_destinationSpot, _fixRotation)
            ? Node.Status.Success
            : Node.Status.Running;
    }
}