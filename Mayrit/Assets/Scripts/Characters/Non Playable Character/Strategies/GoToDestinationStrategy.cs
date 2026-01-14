using UnityEngine;

public class GoToDestinationStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    readonly Spot _destinationSpot;
    private readonly bool _fixRotation;

    public GoToDestinationStrategy(NPCtype npc, Spot destinationSpot, bool fixRotation = false)
    : base(npc)
    {
        _destinationSpot = destinationSpot;
        _fixRotation = fixRotation;
    }

    public override Node.Status Start()
    {
        if (_npc.MovementController.SetDestinationSpot(_destinationSpot))
        {
            if (_npc.CurrentConversationTarget != null || _npc.ConversationRole != INPC.RoleInConversation.None)
            {
                if (_npc.DebugMode)
                    Debug.Log($"[GoToDestinationStrategy.Start()] {_npc.Name} is going to {_destinationSpot.name}. Ending conversation with {_npc.CurrentConversationTarget.Name}.", _npc.GO);

                _npc.EndConversation();
            }

            return Node.Status.Success;
        }
        else
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToDestinationStrategy.Start()] {_npc.Name} could not set destination", _npc.GO);
            return Node.Status.Failure;
        }
    }

    public override Node.Status Update()
    {
        // Fix destination if needed
        if (!_npc.MovementController.IsDestinationSpot(_destinationSpot))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToDestinationStrategy.Update()] {_npc.Name} fixing destination", _npc.GO);

            _npc.MovementController.SetDestinationSpot(_destinationSpot);
        }

        // Success if arrived at destination
        if (_npc.MovementController.HasArrivedAtSpot(_destinationSpot, _fixRotation))
            return Node.Status.Success;
        // Continue if not
        else
            return Node.Status.Running;
    }
}