using UnityEngine;

public class GoToDestinationStrategy : AStrategy
{
    readonly Spot _destination;

    public GoToDestinationStrategy(INPC npc, Spot destination)
    : base(npc)
    {
        _destination = destination;
    }

    public override Node.Status Update()
    {
        // Set destination if not already set
        if (_npc.GetDestinationPos() != _destination.transform.position)
            _npc.SetDestinationSpot(_destination);

        // Success if arrived at destination
        if (_npc.HasArrivedAtDestination())
            return Node.Status.Success;
        // Continue if not
        else
            return Node.Status.Running;
    }
}