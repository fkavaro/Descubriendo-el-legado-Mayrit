using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionStrategy : AStrategy
{
    #region PROPERTIES
    readonly Func<bool> _predicate;
    #endregion

    #region CONSTRUCTOR
    public ConditionStrategy(ANPC<Node> npc, LeafNode leafNode, Func<bool> predicate)
    : base(npc, leafNode)
    {
        _predicate = predicate;
    }
    #endregion

    #region INHERITED METHODS
    public override Node.Status Update()
    {
        return _predicate() ? Node.Status.Success : Node.Status.Failure;
    }
    #endregion
}
