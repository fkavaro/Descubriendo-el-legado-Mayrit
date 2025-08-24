using UnityEngine;

/// <summary>
/// AStrategy defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public abstract class AStrategy
{
    protected readonly ANPC _controller;
    protected readonly IBehaviourControllable _controllable;

    public AStrategy(ANPC controller)
    {
        _controller = controller;
        _controllable = controller._controllable;
    }

    public abstract Node.Status Update();
    public virtual void Reset() { }
}
