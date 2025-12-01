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
        for (int i = 0; i < _children.Count; i++)
        {
            Node iChild = _children[i];
            Status status = iChild.UpdateNode();

            if (status == Status.Success)
            {
                iChild.Reset();
                return status;
            }

            if (status == Status.Running)
            {
                // If a different child was previously running, reset it
                if (_currentChildIdx != i && _currentChildIdx < _children.Count)
                {
                    //_children[_currentChildIdx].Reset();
                    _currentChildIdx = i;
                }

                return status;
            }

            // Failure -> continue to next child
        }

        // No child succeeded
        Reset();
        return Status.Failure;
    }
    #endregion
}