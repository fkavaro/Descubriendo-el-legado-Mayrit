using UnityEngine;

public class InInteriorStrategy<NPCtype> : ATimedNPCStrategy<NPCtype>
where NPCtype : INPC
{
    public InInteriorStrategy(NPCtype npc, float min = 30, float max = 120)
    : base(npc, min, max) { }

    public override Node.Status Start()
    {
        // Deactivate model and agent
        _npc.SetCharacterAndAgentActive(false);

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
