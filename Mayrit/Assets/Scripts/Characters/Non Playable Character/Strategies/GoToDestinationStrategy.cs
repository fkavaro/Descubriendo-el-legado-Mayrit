using UnityEngine;

public class GoToDestinationStrategy : AStrategy
{
    readonly Spot _destinationSpot;
    private readonly bool _fixRotation, _fixPosition;

    public GoToDestinationStrategy(INPC npc, Spot destinationSpot, bool fixRotation = false, bool fixPosition = false)
    : base(npc)
    {
        _destinationSpot = destinationSpot;
        _fixRotation = fixRotation;
        _fixPosition = fixPosition;
    }

    public override Node.Status Start()
    {
        // Set initial destination
        _npc.SetDestinationSpot(_destinationSpot);

        if (_npc.IsDestination(_destinationSpot))
            return Node.Status.Success;
        else
            return Node.Status.Failure;
    }

    public override Node.Status Update()
    {
        // Success if arrived at destination
        if (_npc.HasArrivedAt(_destinationSpot, _fixRotation, _fixPosition))
        {
            _npc.AnimationController.ChangeToIdle();
            return Node.Status.Success;
        }
        // Continue if not
        else
            return Node.Status.Running;
    }
}