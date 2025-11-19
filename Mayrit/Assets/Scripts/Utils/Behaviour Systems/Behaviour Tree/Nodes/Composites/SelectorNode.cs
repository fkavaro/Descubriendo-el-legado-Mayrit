using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SelectorNode is a composite node that that executes its children in sequence.
/// Like a logical OR operation, it will return success when a child return success.
/// Continues to the next child if a child fails.
/// </summary>
public class SelectorNode : Node
{
    #region CONSTRUCTOR
    public SelectorNode(IBehaviourEntity entity, int priority = 0)
    : base(entity, "Selector", priority) { }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        // Priority selector: evaluate children from highest to lowest priority
        for (int i = 0; i < _children.Count; i++)
        {
            var status = _children[i].UpdateNode();

            if (status == Status.Success)
            {
                Reset();
                return Status.Success;
            }

            if (status == Status.Running)
            {
                // If a different child was previously running, reset it (preempt)
                if (_currentChildId != i && _currentChildId < _children.Count)
                {
                    _children[_currentChildId].Reset();
                    _currentChildId = i;
                }

                return Status.Running;
            }

            // Failure -> continue to next child
        }

        // No child succeeded or is running
        Reset();
        return Status.Failure;
    }
    #endregion
}