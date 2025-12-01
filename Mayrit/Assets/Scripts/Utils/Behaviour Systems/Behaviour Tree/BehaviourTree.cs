using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourTree : Node
{
    #region CONSTRUCTORS
    public BehaviourTree(IBehaviourEntity entity, string name = "BehaviourTree")
    : base(entity, name) { }

    public BehaviourTree(IBehaviourEntity entity, Node child, string name = "BehaviourTree")
    : base(entity, name)
    {
        AddChild(child);
    }
    #endregion

    #region INHERITED METHODS
    /// <summary>
    /// Updates all child nodes of the behaviour tree.
    /// Changes when child succeeds.
    /// </summary>
    /// <returns>Success when all children have returned success</returns>
    public override Status UpdateNode()
    {
        while (_currentChildIdx < _children.Count)
        {
            var status = _children[_currentChildIdx].UpdateNode();

            if (status != Status.Success)
                return status;

            _currentChildIdx++;
        }
        return Status.Success;
    }
    #endregion
}