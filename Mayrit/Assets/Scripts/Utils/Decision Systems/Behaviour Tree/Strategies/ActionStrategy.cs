using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionStrategy : AStrategy
{
    readonly Action _action;

    public ActionStrategy(ANPC controller, Action action)
    : base(controller)
    {
        _action = action;
    }

    public override Node.Status Update()
    {
        _action();
        return Node.Status.Success;
    }
}