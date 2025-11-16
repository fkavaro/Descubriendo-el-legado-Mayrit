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
    public virtual Node.Status Start()
    {
        return Node.Status.Success; // Default implementation returns Success
    }
    public virtual Node.Status Update()
    {
        return Node.Status.Success;
    }
    public virtual void Reset() { }
    #endregion
}
