using UnityEngine;

public class InteractionStrategy : ATimedStrategy
{
    INPC _otherNPC;
    bool _initiatorArrived = false;
    bool _targetArrived = false;
    bool _interactionStarted = false;
    Vector3 targetA, targetB;

    public InteractionStrategy(INPC npc, int min = 30, int max = 60)
    : base(npc, min, max) { }

    public override Node.Status Start()
    {
        _otherNPC = (INPC)_npc.CurrentInteractionTarget;

        // Null or handshake is refused
        if (_otherNPC == null || !_otherNPC.TryAcceptInteraction(_npc))
            return Node.Status.Failure;

        // Handshake accepted
        // Compute midpoint and safe target positions offset so NPCs don't overlap
        Vector3 posA = _npc.GO.transform.position;
        Vector3 posB = _otherNPC.GO.transform.position;
        Vector3 midpoint = (posA + posB) * 0.5f;

        Vector3 dir = posB - posA;
        if (dir.sqrMagnitude < 0.0001f)
            dir = _npc.GO.transform.forward;
        dir.Normalize();

        // Determine a comfortable separation using avoidance radii
        float separation = Mathf.Max(0.5f, _npc.AvoidanceRadius + _otherNPC.AvoidanceRadius) + 0.1f;
        float half = separation * 0.5f;

        targetA = midpoint - dir * half;
        targetB = midpoint + dir * half;

        // Command movement to calculated targets
        _npc.MovementController.SetDestination(targetA);
        _otherNPC.MovementController.SetDestination(targetB);

        // Reset flags
        _initiatorArrived = false;
        _targetArrived = false;
        _interactionStarted = false;

        return Node.Status.Success;
    }

    public override Node.Status Update()
    {
        // Check initiator arrival
        if (!_initiatorArrived && _npc.MovementController.HasArrivedAt(targetA))
            _initiatorArrived = true;

        // Check target arrival
        if (!_targetArrived && _otherNPC.MovementController.HasArrivedAt(targetB))
            _targetArrived = true;

        // Both arrived
        if (_initiatorArrived && _targetArrived)
        {
            if (!_interactionStarted)
            {
                _interactionStarted = true;
                Debug.Log($"{_npc.Name} and {_otherNPC.Name} have both arrived to interact.");

                // Both start interaction
                _npc.StartInteraction();
                _otherNPC.StartInteraction();
            }

            // Advance timed logic
            return base.Update();
        }

        return Node.Status.Running;
    }

    public override void OnTimerComplete()
    {
        // End interaction on both
        _npc.EndInteraction();
        _otherNPC.EndInteraction();

        _otherNPC = null;

        Debug.Log($"{_npc.Name} has ended the interaction.");
    }
}