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
        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Check arrival to middle point
        if (!_npc.IsReadyToTalk
        && _npc.MovementController.HasArrivedAt(_middlePoint))
            _npc.IsReadyToTalk = true;

        // Both arrived
        if (_npc.IsReadyToTalk && _otherNPC.IsReadyToTalk)
        {
            if (!_npc.IsTalking)
                _npc.StartConversation();
            else
                // Advance timed logic
                return base.Update();
        }

        return Node.Status.Running;
    }

    public override void OnTimerComplete()
    {
        // End interaction on both
        _npc.EndConversation();
        _otherNPC.EndConversation();
    }
}