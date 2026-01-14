using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementController
{
    #region PROPERTIES HELPERS
    public Spot DestinationSpot => _destinationSpot;
    public Vector3 DestinationPos => _destinationPos;
    #endregion

    #region PROPERTIES
    readonly INPC _npc;
    readonly NavMeshAgent _agent;
    readonly float _positionLeniency; // Allowable leniency when setting destination
    readonly NavMeshQueryFilter _queryFilter;

    Vector3 _destinationPos;
    Spot _destinationSpot;
    #endregion

    #region CONSTRUCTOR
    public NPCMovementController(INPC npc)
    {
        _npc = npc;
        _agent = _npc.Agent;
        _destinationSpot = null;

        // Assign a randomized avoidance priority to reduce symmetric deadlocks between agents
        int priorityOffset = UnityEngine.Random.Range(-_npc.AvoidancePriorityVariance, _npc.AvoidancePriorityVariance + 1);
        _agent.avoidancePriority = Mathf.Clamp(_npc.BaseAvoidancePriority + priorityOffset, 0, 99);

        // Assign randomized walk speed variance
        float speedOffset = UnityEngine.Random.Range(-_npc.WalkSpeedVariance, _npc.WalkSpeedVariance);
        _agent.speed = Mathf.Max(0.1f, _npc.WalkSpeed + speedOffset);

        // Set default values
        _agent.angularSpeed = _npc.RotationSpeed * 100f;
        _agent.stoppingDistance = _npc.StoppingDistance;
        _agent.radius = _npc.AvoidanceRadius;

        // Configure NavMesh query filter for pathfinding calculations
        _queryFilter = new() { areaMask = _agent.areaMask, agentTypeID = _agent.agentTypeID };
        _positionLeniency = _agent.radius + _agent.stoppingDistance + _agent.height;

        // Deactivate agent initially
        _agent.enabled = false;
    }
    #endregion

    #region IF STOPPED METHODS
    /// <summary>
    /// Checks and updates the NPC's NavMeshAgent movement state.
    /// Should be called regularly to ensure proper movement behavior.
    /// </summary>
    public void CheckBehaviourExecution()
    {
        if (!_agent.isOnNavMesh)
            return;

        // Stop moving if execution is paused
        if (_npc.IsExecutionPaused)
            _agent.isStopped = true;
        else
            _agent.isStopped = _npc.IsStopped;
    }

    public void SetIfStopped(bool isStopped)
    {
        if (_agent.isStopped == isStopped) return;

        if (_agent == null)
        {
            Debug.LogError(_npc.Name + ", IsStopped(): NavMeshAgent is null.");
            return;
        }

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError(_npc.Name + ", IsStopped(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        _npc.IsStopped = isStopped;
    }
    #endregion

    #region DESTINATION METHODS
    public bool IsDestination(Vector3 position)
    {
        float tolerance = Mathf.Max(0.1f, _positionLeniency);
        return Vector3.Distance(_destinationPos, position) < tolerance;
    }

    public bool IsDestinationSpot(Spot spot)
    {
        if (spot == null) return false;
        return _destinationSpot == spot;
    }
    #endregion

    #region SET DESTINATION METHODS
    public bool SetDestinationSpot(Spot targetSpot)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        if (targetSpot == null) return false;
        if (IsDestinationSpot(targetSpot)) return true;

        bool success = SetDestination(targetSpot.transform.position);
        if (success)
            _destinationSpot = targetSpot;

        return success;
    }

    public bool SetDestination(Vector3 targetPos)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        if (IsDestination(targetPos)) return true;

        NavMeshPath path = new();

        if (_positionLeniency != 0f)
        {
            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, _positionLeniency, _queryFilter))
                targetPos = hit.position; // Adjust destination to sampled position
            else
            {
                Debug.LogWarning($"[SetDestination()] {_npc.Name} could not sample a valid NavMesh position near the target destination.", _npc.GO);
                return false;
            }
        }

        bool canSetPath = NavMesh.CalculatePath(_agent.transform.position, targetPos, _queryFilter, path);

        if (!canSetPath)
        {
            Debug.LogWarning($"[SetDestination()] {_npc.Name} could not calculate a valid path to the target destination.", _npc.GO);
            return false;
        }

        if (_destinationSpot != null)
        {
            _destinationSpot.SetOccupied(false);
            _destinationSpot = null;
        }

        _destinationPos = targetPos;
        _agent.updateRotation = true;
        SetIfStopped(false);
        _agent.SetPath(path);
        _npc.CharacterModel.SetActive(true);
        _npc.AnimationController.ChangeToWalk();
        return true;
    }
    #endregion

    #region IS CLOSE METHODS
    public bool IsCloseToSpot(Spot targetSpot, float horizontalDistance = 2f, float verticalDistance = 3.5f)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        if (targetSpot == null)
            return false;

        if (!IsDestinationSpot(targetSpot))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"{_npc.Name} checking close distance at unset destination spot.", _npc.GO);
            return false;
        }

        return IsCloseToDestination(horizontalDistance);
    }

    public bool IsCloseToDestination(float checkingDistance = 1f)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        float nearDistance = checkingDistance > 1f ? checkingDistance : _npc.NearDistance;

        return _agent.remainingDistance <= nearDistance;
    }
    #endregion

    #region HAS ARRIVED METHODS
    public bool HasArrivedAtSpot(Spot targetSpot, bool fixRotation = false)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        if (targetSpot == null)
            return false;

        if (!IsDestinationSpot(targetSpot))
        {
            if (_npc.DebugMode)
                Debug.LogWarning($"{_npc.Name} checking arrival at unset destination spot.", _npc.GO);
            return false;
        }

        if (HasArrivedAtDestination())
        {
            targetSpot.SetOccupied(true);

            if (fixRotation && !HasRotationCompleted(targetSpot.WorldDirection))
            {
                SmoothRotation(targetSpot.WorldDirection);
                return false;
            }

            return true;
        }

        return false;
    }

    public bool HasArrivedAtDestination()
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return false;

        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    }
    #endregion

    #region ROTATION METHODS
    public void ForceRotation(Quaternion rotation)
    {
        if (_agent.isOnNavMesh)
            _agent.updateRotation = false; // Disable automatic rotation

        _agent.transform.rotation = rotation;
    }

    public bool HasRotationCompleted(Quaternion targetRotation)
    {
        float currentYaw = _agent.transform.eulerAngles.y;
        float targetYaw = targetRotation.eulerAngles.y;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentYaw, targetYaw));
        return angleDifference < 0.5f; // Threshold for rotation completion
    }

    public void SmoothRotation(Quaternion direction)
    {
        if (_agent.isOnNavMesh)
            _agent.updateRotation = false; // Disable automatic rotation

        // Rotate only in the XZ plane (Y-axis rotation only)
        float currentYaw = _agent.transform.eulerAngles.y;
        float targetYaw = direction.eulerAngles.y;

        float newYaw = Mathf.MoveTowardsAngle(currentYaw, targetYaw, _npc.RotationSpeed * Time.deltaTime);

        Vector3 eulerAngles = _agent.transform.eulerAngles;
        eulerAngles.y = newYaw;
        _agent.transform.eulerAngles = eulerAngles;
    }
    #endregion

    #region PLACEMENT METHODS
    public void PlaceAtSpot(Spot spot)
    {
        if (spot == null)
        {
            Debug.LogWarning("PlaceAtSpot(Spot): spot is null.");
            return;
        }

        PlaceAt(spot.transform.position);
        spot.SetOccupied(true);
    }

    public void PlaceAt(Vector3 position)
    {
        Vector3 placementPos;
        bool sampled = false;

        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            placementPos = hit.position;
            sampled = true;
        }
        else
        {
            placementPos = position;
        }

        _npc.GO.transform.position = placementPos;

        _npc.Agent.enabled = true;

        if (sampled)
            _npc.Agent.Warp(placementPos);
    }
    #endregion

    #region GO TO MIDDLE POINT

    public Vector3 GoToMiddlePoint(INPC otherNPC)
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return Vector3.zero;

        // Compute midpoint and safe target positions offset so NPCs don't overlap
        Vector3 posA = _npc.GO.transform.position;
        Vector3 posB = otherNPC.GO.transform.position;
        Vector3 midPoint = (posA + posB) * 0.5f;

        Vector3 direction = posB - posA;
        if (direction.sqrMagnitude < 0.0001f)
            direction = _npc.GO.transform.forward;
        direction.Normalize();

        // Determine a comfortable separation using avoidance radii
        float separation = Mathf.Max(0.5f, _npc.AvoidanceRadius + otherNPC.AvoidanceRadius);
        float half = separation * 0.5f;

        Vector3 correctedMidPoint = midPoint - direction * half;

        // Prefer a reachable point on the NavMesh. Try corrected midpoint first,
        // then raw midpoint, then sample along the segment between the two NPCs.
        Vector3 destination = correctedMidPoint;

        if (CanReachPosition(correctedMidPoint, out Vector3 reachablePos))
        {
            destination = reachablePos;
        }
        else if (CanReachPosition(midPoint, out reachablePos))
        {
            destination = reachablePos;
        }
        else
        {
            bool found = false;
            // Sample several points along the line between the two NPCs to find a reachable spot
            for (int i = 1; i <= 9; i++)
            {
                float t = i / 10f;
                Vector3 sample = Vector3.Lerp(posA, posB, t);
                if (CanReachPosition(sample, out reachablePos))
                {
                    destination = reachablePos;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // As a last resort, remain in place (avoid commanding unreachable destination)
                destination = _agent != null ? _agent.transform.position : correctedMidPoint;
            }
        }

        // Command movement to the chosen reachable destination
        SetDestination(destination);

        return destination;
    }

    public bool CanReachPosition(Vector3 targetPos, out Vector3 reachablePos)
    {
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hitLocation, _npc.MaxSamplingDistance, NavMesh.AllAreas))
        {
            reachablePos = hitLocation.position;
            return true;
        }
        else
        {
            reachablePos = Vector3.zero;
            return false;
        }
    }
    #endregion

    public void Reset()
    {
        _destinationSpot = null;
        _destinationPos = Vector3.zero;
    }
}
