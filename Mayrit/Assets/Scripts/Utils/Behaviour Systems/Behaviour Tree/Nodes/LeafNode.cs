using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// LeafNode is a node that has no children and executes an specific action.
/// </summary>
public class LeafNode : Node
{
    #region PROPERTIES
    readonly AStrategy _strategy;
    #endregion

    #region CONSTRUCTOR
    public LeafNode(IBehaviourEntity entity, string name, AStrategy strategy, int priority = 0)
    : base(entity, name, priority)
    {
        _strategy = strategy;
    }
    #endregion

    #region INHERITED METHODS
    public override Status UpdateNode()
    {
        return _strategy.Update();
    }

    public override void Reset()
    {
        _strategy.Reset();
    }
    #endregion
}