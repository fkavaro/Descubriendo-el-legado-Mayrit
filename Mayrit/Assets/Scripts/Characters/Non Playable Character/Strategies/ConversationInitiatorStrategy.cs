using UnityEngine;

public class ConversationInitiatorStrategy : ATimedStrategy
{
    INPC _otherNPC;

    public ConversationInitiatorStrategy(INPC npc, int min = 30, int max = 60)
    : base(npc, min, max) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentConversationTarget;

        if (_otherNPC == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ConversationInitiatorStrategy.Start()] {_npc.Name} trying to talk to null NPC");

            return Node.Status.Failure;
        }

        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        if (_npc.DebugMode)
            Debug.Log($"[ConversationInitiatorStrategy.Start()] {_npc.Name} initiating conversation with {_otherNPC.Name}");

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        // Continue conversation during certain time
        _npc.Talk();
        _npc.GO.transform.LookAt(_otherNPC.GO.transform.position);

        // Continue timing
        return base.Update();
    }

    bool IsOtherStillInConversation()
    {
        if (!_otherNPC.IsStillInConversation(_npc))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[ConversationInitiatorStrategy] {_npc.Name} found that {_otherNPC.Name} is no longer in conversation.");

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