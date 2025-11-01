using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SelectorNode is a composite node that that executes its children randomly.
/// Like a logical OR operation, it will return success when a child return success.
/// </summary>
public class RanndomSelectorNode : PrioritySelectorNode
{
    #region CONSTRUCTOR
    public RanndomSelectorNode(IBehaviourEntity entity, int priority = 0)
    : base(entity, priority) { }
    #endregion

    #region INHERITED METHODS
    protected override List<Node> SortChildren()
    {
        return _children.Shuffle().ToList();
    }
    #endregion
}