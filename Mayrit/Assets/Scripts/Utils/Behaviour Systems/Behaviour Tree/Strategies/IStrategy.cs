using UnityEngine;

public interface IStrategy
{
    public Node.Status Start();
    public Node.Status Update();
    public virtual void Reset() { }
}
