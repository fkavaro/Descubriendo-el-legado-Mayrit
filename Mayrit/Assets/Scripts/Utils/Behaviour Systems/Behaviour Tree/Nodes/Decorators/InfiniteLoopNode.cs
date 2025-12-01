using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class InfiniteLoopNode : Node
{
    #region CONSTRUCTORs
    private readonly Node _child;
    #endregion

    #region CONSTRUCTORS
    public InfiniteLoopNode(IBehaviourEntity entity)
    : base(entity, "InfiniteLoop") { }

    public InfiniteLoopNode(IBehaviourEntity entity, Node child)
    : base(entity, "InfiniteLoop")
    {
        AddChild(child); // Use the AddChild method to set the child
        _child = _children[0]; // Store a direct reference for easier access
    }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        if (_child == null)
            return Status.Failure; // If there is no child, the node fails

        _child.UpdateNode();

        // // Child has finished
        // if (_child.UpdateNode() != Status.Running)
        //     _child.Reset();

        return Status.Running;
    }
    #endregion
}