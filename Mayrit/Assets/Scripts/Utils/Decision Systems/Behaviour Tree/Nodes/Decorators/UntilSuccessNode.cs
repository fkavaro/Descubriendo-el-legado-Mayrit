using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UntilSuccessNode is a node that continues running its only child as long as it doesn't return success.
/// </summary>
public class UntilSuccessNode : Node
{
    #region PROPERTIES
    private readonly Node _child;
    #endregion

    #region CONSTRUCTOR
    public UntilSuccessNode(IBehaviourEntity entity, Node child, int priority = 0)
    : base(entity, "UntilSuccess", priority)
    {
        AddChild(child);
        _child = child;
    }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        if (_child.UpdateNode() == Status.Success)
        {
            Reset();
            return Status.Success;
        }
        return Status.Running;
    }
    #endregion
}