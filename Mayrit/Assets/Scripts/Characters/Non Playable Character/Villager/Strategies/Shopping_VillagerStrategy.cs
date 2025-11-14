using UnityEngine;

// TODO: implement with a repetition node in the behavior tree?
public class Shopping_VillagerStrategy : AStrategy
{
    /// <summary>
    /// Number of purchases to make during shopping.
    /// </summary>
    readonly int _puchasesNum;
    int _purchasesMade;

    Spot _stall;

    public Shopping_VillagerStrategy(INPC npc, int min = 2, int max = 10)
    : base(npc)
    {
        _puchasesNum = Random.Range(min, max);
        _purchasesMade = 0;
        _stall = null;
    }

    // public override Node.Status Update()
    // {
    //     if (_purchasesMade < _puchasesNum)
    //     {
    //         if (_stall == null)
    //         {
    //             _stall = TownManager.Instance.GetRandomMarketStallSpot();

    //             _npc.SetDestinationSpot(_stall);

    //             if (_npc.HasArrivedAt(_stall))
    //             {
    //                 _stall = null;
    //                 _purchasesMade++;
    //             }
    //         }

    //         return Node.Status.Running;
    //     }
    //     else
    //     {
    //         // Reset for next shopping trip
    //         _purchasesMade = 0;
    //         return Node.Status.Success;
    //     }
    // }
}
