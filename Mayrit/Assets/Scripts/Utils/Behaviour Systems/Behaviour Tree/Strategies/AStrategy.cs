using UnityEngine;

/// <summary>
/// AStrategy defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public abstract class AStrategy
{
    #region PROPERTIES
    protected readonly INPC _npc;
    #endregion

    #region CONSTRUCTOR
    public AStrategy(INPC npc)
    {
        _npc = npc;
    }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    public abstract Node.Status Update();
    public virtual void Reset() { }
    #endregion
}
