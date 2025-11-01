using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourTree : Node
{
    #region CONSTRUCTORs
    public BehaviourTree(IBehaviourEntity entity, GameObject entityGO, string name = "BehaviourTree")
    : base(entity, name) { }

    public BehaviourTree(IBehaviourEntity entity, GameObject entityGO, Node child, string name = "BehaviourTree")
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
        while (_currentChildId < _children.Count)
        {
            var status = _children[_currentChildId].UpdateNode();

            if (status != Status.Success)
                return status;

            _currentChildId++;
        }
        return Status.Success;
    }
    #endregion
}