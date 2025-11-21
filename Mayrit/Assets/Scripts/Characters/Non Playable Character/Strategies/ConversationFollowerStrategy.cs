using UnityEngine;

public class ConversationFollowerStrategy : AStrategy
{
    INPC _otherNPC;
    Vector3 _middlePoint;

    public ConversationFollowerStrategy(INPC npc)
    : base(npc) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentInteractionTarget;

        if (_otherNPC == null)
            return Node.Status.Failure;

        _middlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

        if (_middlePoint == null)
            return Node.Status.Failure;
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
                    // Look at other npc
                    _npc.GO.transform.LookAt(_otherNPC.GO.transform.position);
            }

            // Success when conversation ends (when current interaction target is null)
            // Depeds on the other, the initiator, to end the conversation
            if (_npc.CurrentInteractionTarget == null)
                return Node.Status.Success;

            return Node.Status.Running;
        }
        else
        {
            // Other NPC is no longer available: end conversation
            _npc.EndConversation();
            return Node.Status.Failure;
        }
    }
}
