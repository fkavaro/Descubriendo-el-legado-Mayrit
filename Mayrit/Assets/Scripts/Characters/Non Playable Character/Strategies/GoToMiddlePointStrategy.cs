using System;
using UnityEngine;

public class GoToMiddlePointStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    INPC _otherNPC;
    Vector3 _middlePoint;

    public GoToMiddlePointStrategy(NPCtype npc)
    : base(npc) { }

    public override Node.Status Start()
    {
        _otherNPC = _npc.CurrentConversationTarget;

        if (_otherNPC == null)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Start()] {_npc.Name} trying to talk to null NPC", _npc.GO);
            return Node.Status.Failure;
        }

        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        _middlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

        if (_middlePoint == Vector3.zero)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Start()] {_npc.Name} could not calculate middle point to {_otherNPC.Name}", _npc.GO);

            return Node.Status.Failure;
        }

        if (_npc.DebugMode)
            Debug.Log($"[GoToMiddlePointStrategy.Start()] {_npc.Name} moving to talk to {_otherNPC.Name} as {_npc.ConversationRole}", _npc.GO);

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        if (_npc.MovementController.HasArrivedAtDestination())
        {
            _npc.AnimationController.ChangeToIdle();
            _npc.IsReadyToTalk = true;
        }
        else
        {
            _npc.AnimationController.ChangeToWalk();
        }

        // Success if both are ready to talk
        if (_npc.IsReadyToTalk && _otherNPC.IsReadyToTalk)
            return Node.Status.Success;

        return Node.Status.Running;
    }

    bool IsOtherStillInConversation()
    {
        if (!_otherNPC.IsStillInConversation(_npc))
        {
            if (_npc.DebugMode)
                Debug.Log($"[GoToMiddlePointStrategy] {_npc.Name} found that {_otherNPC.Name} is no longer in conversation.", _npc.GO);

            _npc.EndConversation();
            return false;
        }

        return true;
    }
}
