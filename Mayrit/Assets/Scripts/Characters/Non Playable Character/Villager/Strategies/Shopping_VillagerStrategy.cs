using UnityEngine;

public class Shopping_VillagerStrategy : AStrategy
{
    /// <summary>
    /// Number of purchases to make during shopping.
    /// </summary>
    readonly int _puchasesNum;

    int _purchasesMade;

    public Shopping_VillagerStrategy(INPC npc, int min = 2, int max = 10)
    : base(npc)
    {
        _puchasesNum = Random.Range(min, max);
        _purchasesMade = 0;
    }

    public override Node.Status Update()
    {
        if (_purchasesMade < _puchasesNum)
        {
            // Simulate a purchase (could be expanded with actual logic)
            _purchasesMade++;
            return Node.Status.Running;
        }
        else
        {
            // Reset for next shopping trip
            _purchasesMade = 0;
            return Node.Status.Success;
        }
    }
}
