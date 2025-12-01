using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// PrioritySelectorNode is a composite node that that executes its children in descencing priority.
/// Like a logical OR operation, it will return success when a child return success.
/// </summary>
public class PrioritySelectorNode : SelectorNode
{
    #region PROPERTIES
    List<Node> sortedChildren;
    List<Node> SortedChildren => sortedChildren ??= SortChildren();
    #endregion

    #region CONSTRUCTOR
    public PrioritySelectorNode(IBehaviourEntity entity, int priority = 0)
    : base(entity, priority) { }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        // Priority selector over sorted children (preemptive)
        for (int i = 0; i < SortedChildren.Count; i++)
        {
            var child = SortedChildren[i];
            var status = child.UpdateNode();

            if (status == Status.Success)
            {
                Reset();
                return Status.Success;
            }

            if (status == Status.Running)
            {
                if (_currentChildIdx != i && _currentChildIdx < SortedChildren.Count)
                {
                    // If previously a different child was running, reset it
                    SortedChildren[_currentChildIdx].Reset();
                    _currentChildIdx = i;
                }
                return Status.Running;
            }

            // Failure -> continue
        }

        Reset();
        return Status.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        sortedChildren = null;
    }
    #endregion

    #region VIRTUAL METHODS
    protected virtual List<Node> SortChildren()
    {
        return _children.OrderByDescending(child => child._priority).ToList();
    }
    #endregion
}