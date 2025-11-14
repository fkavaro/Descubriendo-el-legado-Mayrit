using UnityEngine;

public class GoToDestinationStrategy : AStrategy
{
    readonly Spot _destinationSpot;

    public GoToDestinationStrategy(INPC npc, Spot destinationSpot)
    : base(npc)
    {
        _destinationSpot = destinationSpot;
    }

    public override Node.Status Update()
    {
        // Set destination if not already set
        if (!_npc.IsDestination(_destinationSpot))
            _npc.SetDestinationSpot(_destinationSpot);

        // Success if arrived at destination
        if (_npc.HasArrivedAt(_destinationSpot))
        {
            _npc.AnimationController.ChangeToIdle();
            return Node.Status.Success;
        }
        // Continue if not
        else
            return Node.Status.Running;
    }
}