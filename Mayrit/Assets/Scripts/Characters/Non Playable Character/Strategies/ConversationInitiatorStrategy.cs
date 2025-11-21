using UnityEngine;

public class ConversationInitiatorStrategy : ATimedStrategy
{
    INPC _otherNPC;
    Vector3 _middlePoint;

    public ConversationInitiatorStrategy(INPC npc, int min = 30, int max = 60)
    : base(npc, min, max) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentInteractionTarget;

        if (_otherNPC == null)
            return Node.Status.Failure;

        _middlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

        if (_middlePoint == null)
        {
            Debug.LogWarning("Failed to calculate middle point for conversation between " + _npc.Name + " and " + _otherNPC.Name);
            return Node.Status.Failure;
        }
        else
            return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        if (_otherNPC.IsAvailableForConversation())
        {
            // Check arrival to middle point
            if (!_npc.IsReadyToTalk && _npc.MovementController.HasArrivedAt(_middlePoint))
                _npc.IsReadyToTalk = true;

            // Both arrived
            if (_npc.IsReadyToTalk && _otherNPC.IsReadyToTalk)
            {
                if (!_npc.IsTalking)
                    _npc.StartConversation();
                else
                {
                    // Look at other npc
                    _npc.GO.transform.LookAt(_otherNPC.GO.transform.position);
                    // Advance timed logic
                    return base.Update();
                }
            }

            return Node.Status.Running;
        }
        else
        {
            // Other NPC is no longer available: end conversation
            _npc.EndConversation();
            Debug.Log("Conversation aborted between " + _npc.Name + " and " + _otherNPC.Name);
            return Node.Status.Failure;
        }
    }

    public override void OnTimerComplete()
    {
        // End interaction on both
        _npc.EndConversation();
        _otherNPC.EndConversation();
    }
}