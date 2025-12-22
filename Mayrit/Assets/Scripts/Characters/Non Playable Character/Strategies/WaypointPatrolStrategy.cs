using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// WaypointPatrolStrategy is a strategy for patrolling between a list of points using a NavMeshAgent.
/// </summary>
public class WaypointPatrolStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : ANPC<Node>
{
    #region PROPERTIES
    readonly List<Transform> _patrolPoints;
    int _currentPatrolPointIndex;
    bool _isPathCalculated;
    #endregion

    #region CONSTRUCTOR
    public WaypointPatrolStrategy(NPCtype npc, List<Transform> patrolPoints)
    : base(npc)
    {
        _patrolPoints = patrolPoints;
    }
    #endregion

    #region INHERITED METHODS
    public override Node.Status Update()
    {
        if (_currentPatrolPointIndex >= _patrolPoints.Count)
            return Node.Status.Success;

        var target = _patrolPoints[_currentPatrolPointIndex];
        _npc.MovementController.SetDestination(target.position);
        _npc.GO.transform.LookAt(target);

        if (_isPathCalculated && _npc.MovementController.HasArrivedAtDestination())
        {
            _currentPatrolPointIndex++;
            _isPathCalculated = false;
        }

        if (_npc.MovementController.IsPathPending())
        {
            _isPathCalculated = true;
        }

        return Node.Status.Running;
    }

    public override void Reset()
    {
        _currentPatrolPointIndex = 0;
    }
    #endregion
}
