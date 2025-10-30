using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UntilFailNode is a node that continues running its only child as long as it doesn't return failure.
/// </summary>
public class UntilFailNode : Node
{
    #region PROPERTIES
    private readonly Node _child;
    #endregion

    #region CONSTRUCTOR
    public UntilFailNode(IBehaviourEntity<ABehaviourSystem> entity, GameObject entityGO, Node child, int priority = 0)
    : base(entity, entityGO, "UntilFail", priority)
    {
        AddChild(child); // Use the AddChild method to set the child
        _child = child;
    }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        if (_child.UpdateNode() == Status.Failure)
        {
            Reset();
            return Status.Failure;
        }
        return Status.Running;
    }
    #endregion
}
