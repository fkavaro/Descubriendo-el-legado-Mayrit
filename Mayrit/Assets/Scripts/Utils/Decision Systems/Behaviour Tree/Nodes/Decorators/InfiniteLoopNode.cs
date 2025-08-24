using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class InfiniteLoopNode : Node
{
    private readonly Node _child;

    public InfiniteLoopNode(ABehaviourController controller)
    : base(controller, "InfiniteLoop") { }

    public InfiniteLoopNode(ABehaviourController controller, Node child)
    : base(controller, "InfiniteLoop")
    {
        AddChild(child); // Use the AddChild method to set the child
        _child = children[0]; // Store a direct reference for easier access
    }

    public override Status UpdateNode()
    {
        if (_child == null)
            return Status.Failure; // If there is no child, the node fails

        // Child has finished
        if (_child.UpdateNode() != Status.Running)
            _child.Reset();

        return Status.Running;
    }
}