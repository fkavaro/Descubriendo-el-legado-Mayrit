using UnityEngine;

/// <summary>
/// AStrategy defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public abstract class AStrategy
{
    protected readonly ANPC _npc;
    protected readonly IBehaviourControllable _controllable;
    protected bool DebugMode => _controllable.BehaviourController._debugMode;

    public AStrategy(ANPC npc)
    {
        _npc = npc;
        _controllable = npc._controllable;
    }

    public abstract Node.Status Update();
    public virtual void Reset() { }
}
