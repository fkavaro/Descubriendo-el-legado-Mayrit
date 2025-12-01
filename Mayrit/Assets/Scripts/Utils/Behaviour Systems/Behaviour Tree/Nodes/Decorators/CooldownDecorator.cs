using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// CooldownDecorator prevents its child from running until a cooldown has elapsed
/// since the child's last successful run.
/// If the cooldown hasn't elapsed it returns Failure so other branches can run.
/// </summary>
public class CooldownDecorator : Node
{
    private readonly float _cooldown;
    private float _lastUse = -Mathf.Infinity;

    public CooldownDecorator(IBehaviourEntity entity, float cooldown, int priority = 0)
    : base(entity, "Cooldown(" + cooldown + ")", priority)
    {
        _cooldown = cooldown;
    }

    public override Status UpdateNode()
    {
        if (_children == null || _children.Count == 0)
            return Status.Failure;

        // If cooldown not elapsed, don't run the child (allow other branches)
        if (Time.time - _lastUse < _cooldown)
            return Status.Failure;

        // Run the child
        var child = _children[0];
        var status = child.UpdateNode();

        // If child succeeded, record usage time
        if (status == Status.Success)
            _lastUse = Time.time;

        return status;
    }
}
