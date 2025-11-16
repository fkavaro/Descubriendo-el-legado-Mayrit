using UnityEngine;

public class InteractStrategy : AStrategy
{
    public InteractStrategy(INPC npc)
    : base(npc) { }

    public override Node.Status Update()
    {
        throw new System.NotImplementedException();
    }
}

