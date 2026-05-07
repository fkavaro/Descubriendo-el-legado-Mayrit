using System;
using UnityEngine;

public class InInteriorStrategy<NPCtype> : ATimedNPCStrategy<NPCtype>
where NPCtype : INPC
{
    readonly Spot _interiorSpot;

    public InInteriorStrategy(NPCtype npc, Spot interiorSpot, float min = 30, float max = 120)
    : base(npc, min, max)
    {
        _interiorSpot = interiorSpot;
    }

    public override Node.Status Start()
    {
        // Deactivate model and agent
        _npc.SetCharacterAndAgentActive(false);

        // Place at the spot if not already there
        if (_interiorSpot != null && !_npc.MovementController.IsNearPosition(_interiorSpot.transform.position))
            _npc.MovementController.PlaceAtSpot(_interiorSpot, true);

        if (_npc.CharacterModel.activeSelf == false && !_npc.Agent.enabled)
            return Node.Status.Success;
        else
            return Node.Status.Failure;
    }

    public override void OnTimerComplete()
    {
        // Reactivate model and agent
        _npc.SetCharacterAndAgentActive(true);
    }
}
