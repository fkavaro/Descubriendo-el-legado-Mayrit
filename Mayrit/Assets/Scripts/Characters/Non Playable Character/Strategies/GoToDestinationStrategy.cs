using UnityEngine;

public class GoToDestinationStrategy : AStrategy
{
    public GoToDestinationStrategy(INPC npc)
    : base(npc) { }

    public override Node.Status Update()
    {
        throw new System.NotImplementedException();
    }
}