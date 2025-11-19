using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SequenceNode is a composite node that executes its children in sequence.
/// Like a logical AND operation, it will return success only if all its children return success.
/// Continues to the next child if a child succeeds.
/// </summary>
public class SequenceNode : Node
{
    #region CONSTRUCTOR
    public SequenceNode(IBehaviourEntity entity, int priority = 0)
    : base(entity, "Sequence", priority) { }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        // Execute children in order, preserving progress across ticks using
        // `_currentChildId`. If a child returns Running we must resume from
        // that child next tick instead of restarting from the first child.
        while (_currentChildId < _children.Count)
        {
            var status = _children[_currentChildId].UpdateNode();

            if (status == Status.Running)
                return Status.Running;

            if (status == Status.Failure)
            {
                Reset();
                return Status.Failure;
            }

            // status == Success -> advance to next child
            _currentChildId++;
        }

        // All children succeeded
        Reset();
        return Status.Success;
    }
    #endregion
}