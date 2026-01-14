using System;
using UnityEngine;

public class GoToMiddlePointStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    private const float MIDPOINT_RECALC_DISTANCE = 1f; // Recalculate if NPCs drift apart
    private const float UPDATE_INTERVAL = 0.5f; // Check every 0.5 seconds

    INPC _otherNPC;
    Vector3 _lastMiddlePoint;
    float _timeSinceLastUpdate;
    bool _isMoving;

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

        _lastMiddlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

        if (_lastMiddlePoint == Vector3.zero)
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"[GoToMiddlePointStrategy.Start()] {_npc.Name} could not calculate middle point to {_otherNPC.Name}", _npc.GO);

            return Node.Status.Failure;
        }

        _isMoving = true;
        _timeSinceLastUpdate = 0f;

        if (_npc.DebugMode)
            Debug.Log($"[GoToMiddlePointStrategy.Start()] {_npc.Name} moving to talk to {_otherNPC.Name} as {_npc.ConversationRole}", _npc.GO);

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Failure if other NPC is no longer in conversation
        if (!IsOtherStillInConversation())
            return Node.Status.Failure;

        // Update timer for periodic recalculation
        _timeSinceLastUpdate += Time.deltaTime;

        // Check if arrived at destination
        bool hasArrived = _npc.MovementController.HasArrivedAtDestination();

        if (hasArrived)
        {
            // Handle arrival - set idle animation once
            if (_isMoving)
            {
                _npc.AnimationController.ChangeToIdle();
                _isMoving = false;
            }

            _npc.IsReadyToTalk = true;
        }
        else
        {
            // Handle movement - set walk animation once
            if (!_isMoving)
            {
                _npc.AnimationController.ChangeToWalk();
                _isMoving = true;
            }

            // Periodically recalculate middle point if NPCs have drifted apart
            if (_timeSinceLastUpdate >= UPDATE_INTERVAL)
            {
                Vector3 newMiddlePoint = _npc.MovementController.GoToMiddlePoint(_otherNPC);

                // If middle point changed significantly, update destination
                if (Vector3.Distance(_lastMiddlePoint, newMiddlePoint) > MIDPOINT_RECALC_DISTANCE)
                {
                    _lastMiddlePoint = newMiddlePoint;
                    if (_npc.DebugMode)
                        Debug.Log($"[GoToMiddlePointStrategy.Update()] {_npc.Name} recalculating middle point, distance drifted.", _npc.GO);
                }

                _timeSinceLastUpdate = 0f;
            }
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
