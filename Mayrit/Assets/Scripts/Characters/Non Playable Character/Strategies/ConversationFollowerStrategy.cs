using UnityEngine;

public class ConversationFollowerStrategy : AStrategy
{
    INPC _otherNPC;
    bool _otherFinishedTalking;

    public ConversationFollowerStrategy(INPC npc)
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

        // Follow conversation
        _npc.Talk();
        _npc.GO.transform.LookAt(_otherNPC.GO.transform.position);
        return Node.Status.Running;
    }

    bool IsOtherStillInConversation()
    {
        if (!_otherNPC.IsStillInConversation(_npc))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ConversationFollowerStrategy] {_npc.Name} found that {_otherNPC.Name} is no longer in conversation.");

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
