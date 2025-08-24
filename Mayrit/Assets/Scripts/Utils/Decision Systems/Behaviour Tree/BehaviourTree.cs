using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree(ABehaviourController controller, string name = "BehaviourTree")
    : base(controller, name) { }

    public BehaviourTree(ABehaviourController controller, Node child, string name = "BehaviourTree")
    : base(controller, name)
    {
        AddChild(child);
    }

    // protected override void DebugDecision()
    // {
    //     if (_currentChildId < children.Count)
    //         controller.stateText.text = children[_currentChildId].name;
    //     else
    //         controller.stateText.text = "None";
    // }

    public override Status UpdateNode()
    {
        while (_currentChildId < children.Count)
        {
            var status = children[_currentChildId].UpdateNode();

            if (status != Status.Success)
                return status;

            _currentChildId++;
        }
        return Status.Success;
    }
}