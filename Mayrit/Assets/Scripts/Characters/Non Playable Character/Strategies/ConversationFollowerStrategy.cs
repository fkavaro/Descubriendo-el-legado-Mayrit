using UnityEngine;

public class ConversationFollowerStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    INPC _otherNPC;
    bool _otherFinishedTalking;

    public ConversationFollowerStrategy(NPCtype npc)
    : base(npc) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentConversationTarget;

        if (_otherNPC == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ConversationFollowerStrategy.Start()] {_npc.Name} is being talked to by null NPC");

            return Node.Status.Failure;
        }

        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        // Subscribe to conversation end event
        _otherNPC.ConversationFinishedEvent += OnConversationFinished;

        if (_npc.DebugMode)
            Debug.Log($"[ConversationFollowerStrategy.Start()] {_npc.Name} is being talked to by {_otherNPC.Name}");

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        // Success if other finished talking
        if (_otherFinishedTalking)
        {
            // Unsubscribe from conversation end event
            _otherNPC.ConversationFinishedEvent -= OnConversationFinished;

            _npc.EndConversation();

            return Node.Status.Success;
        }

        _npc.Talk();

        // Keep facing other NPC (XZ plane only)
        Vector3 targetPosition = _otherNPC.GO.transform.position;
        targetPosition.y = _npc.GO.transform.position.y;
        _npc.GO.transform.LookAt(targetPosition);

        return Node.Status.Running;
    }

    bool IsOtherStillInConversation()
    {
        if (!_otherNPC.IsStillInConversation(_npc))
        {
            if (_npc.DebugMode)
                Debug.Log($"[ConversationFollowerStrategy] {_npc.Name} found that {_otherNPC.Name} is no longer in conversation.");

            _npc.EndConversation();
            return false;
        }

        return true;
    }

    void OnConversationFinished()
    {
        _otherFinishedTalking = true;
    }
}
