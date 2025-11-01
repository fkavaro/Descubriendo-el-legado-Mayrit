using UnityEngine;

/// <summary>
/// AStrategy defines the contract for all strategies used in the behaviour tree nodes.
/// </summary>
public abstract class AStrategy
{
    #region PROPERTIES
    protected readonly ANPC<Node> _npc;
    protected readonly LeafNode _leafNode;
    #endregion

    #region CONSTRUCTOR
    public AStrategy(ANPC<Node> npc, LeafNode leafNode)
    {
        _npc = npc;
        _leafNode = leafNode;
    }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    public abstract Node.Status Update();
    public virtual void Reset() { }
    #endregion
}
