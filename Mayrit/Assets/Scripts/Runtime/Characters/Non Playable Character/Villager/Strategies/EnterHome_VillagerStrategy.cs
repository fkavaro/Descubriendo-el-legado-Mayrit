using UnityEngine;

public class EnterHome_VillagerStrategy : ANPCStrategy<Villager>
{
    readonly Villager _villager;

    public EnterHome_VillagerStrategy(Villager villager)
    : base(villager)
    {
        _villager = villager;
    }

    public override Node.Status Start()
    {
        _villager.ReturnToPool();

        if (_villager.gameObject.activeSelf == false)
            return Node.Status.Success;
        else
            return Node.Status.Failure;
    }
}
