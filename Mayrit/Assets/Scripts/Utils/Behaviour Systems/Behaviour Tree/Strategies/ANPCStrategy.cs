using UnityEngine;

/// <summary>
/// AStrategy defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public abstract class ANPCStrategy<NPCtype> : IStrategy
where NPCtype : INPC
{
    #region PROPERTIES
    protected readonly NPCtype _npc;
    #endregion

    #region CONSTRUCTOR
    public ANPCStrategy(NPCtype npc)
    {
        _npc = npc;
    }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    public virtual Node.Status Start()
    {
        return Node.Status.Success;
    }
    public virtual Node.Status Update()
    {
        return Node.Status.Success;
    }
    public virtual void Reset() { }
    #endregion
}
