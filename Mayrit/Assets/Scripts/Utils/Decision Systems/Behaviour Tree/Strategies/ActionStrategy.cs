using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionStrategy : AStrategy
{
    #region PROPERTIES
    readonly Action _action;
    #endregion

    #region CONSTRUCTOR
    public ActionStrategy(ANPC<Node> npc, LeafNode leafNode, Action action)
    : base(npc, leafNode)
    {
        _action = action;
    }
    #endregion

    #region INHERITED METHODS
    public override Node.Status Update()
    {
        _action();
        return Node.Status.Success;
    }
    #endregion
}