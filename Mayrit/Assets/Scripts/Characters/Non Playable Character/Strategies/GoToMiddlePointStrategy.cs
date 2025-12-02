using System;
using UnityEngine;

public class GoToMiddlePointStrategy : AStrategy
{
    INPC _otherNPC;
    Vector3 _middlePoint;

    public GoToMiddlePointStrategy(INPC npc)
    : base(npc) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentConversationTarget;

        if (_otherNPC == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Start()] {_npc.Name} trying to talk to null NPC");
            return Node.Status.Failure;
        }

        _middlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

        if (_middlePoint == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Start()] {_npc.Name} could not calculate middle point to {_otherNPC.Name}");

            return Node.Status.Failure;
        }

        if (_npc.DebugMode)
            Debug.Log($"[GoToMiddlePointStrategy.Start()] {_npc.Name} moving to talk to {_otherNPC.Name} as {_npc.ConversationRole}");

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer available
        if (!_otherNPC.IsAvailableForConversation())
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Update()] {_npc.Name} found that {_otherNPC.Name} is no longer available for conversation.");
            return Node.Status.Failure;
        }

        // Success if arrived at middle point
        if (_npc.MovementController.HasArrivedAt(_middlePoint))
        {
            _npc.AnimationController.ChangeToIdle();
            _npc.IsReadyToTalk = true;
        }

        // Both are ready to talk
        if (_npc.IsReadyToTalk && _otherNPC.IsReadyToTalk)
        {
            return Node.Status.Success;
        }

        return Node.Status.Running;
    }
}
