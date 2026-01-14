using UnityEngine;

public class ConversationInitiatorStrategy<NPCtype> : ATimedNPCStrategy<NPCtype>
where NPCtype : INPC
{
    INPC _otherNPC;

    public ConversationInitiatorStrategy(NPCtype npc, int min = 30, int max = 60)
    : base(npc, min, max) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentConversationTarget;

        if (_otherNPC == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ConversationInitiatorStrategy.Start()] {_npc.Name} trying to talk to null NPC", _npc.GO);

            return Node.Status.Failure;
        }

        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        _npc.Talk();

        if (_npc.DebugMode)
            Debug.Log($"[ConversationInitiatorStrategy.Start()] {_npc.Name} initiating conversation with {_otherNPC.Name}", _npc.GO);

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        // Keep facing other NPC (XZ plane only)
        Vector3 targetPosition = _otherNPC.GO.transform.position;
        targetPosition.y = _npc.GO.transform.position.y;
        _npc.GO.transform.LookAt(targetPosition);

        // Continue timing
        return base.Update();
    }

    bool IsOtherStillInConversation()
    {
        if (!_otherNPC.IsStillInConversation(_npc))
        {
            if (_npc.DebugMode)
                Debug.Log($"[ConversationInitiatorStrategy] {_npc.Name} found that {_otherNPC.Name} is no longer in conversation.", _npc.GO);

            _npc.EndConversation();
            return false;
        }

        return true;
    }

    public override void OnTimerComplete()
    {
        _npc.EndConversation();
    }
}