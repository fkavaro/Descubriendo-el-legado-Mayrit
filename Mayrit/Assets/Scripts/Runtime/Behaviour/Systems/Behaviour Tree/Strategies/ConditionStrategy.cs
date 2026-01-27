using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionStrategy : IStrategy
{
    #region PROPERTIES
    readonly Func<bool> _predicate;
    #endregion

    #region CONSTRUCTOR
    public ConditionStrategy(Func<bool> predicate)
    {
        _predicate = predicate;
    }
    #endregion

    #region INHERITED METHODS
    public Node.Status Start()
    {
        return Node.Status.Success;
    }

    public Node.Status Update()
    {
        return _predicate() ? Node.Status.Success : Node.Status.Failure;
    }
    #endregion
}
