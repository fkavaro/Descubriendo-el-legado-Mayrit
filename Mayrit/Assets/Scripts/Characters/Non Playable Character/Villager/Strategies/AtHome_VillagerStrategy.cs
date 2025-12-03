using UnityEngine;

public class AtHome_VillagerStrategy : ANPCStrategy<Villager>
{
    readonly Villager _villager;
    public AtHome_VillagerStrategy(Villager villager)
    : base(villager)
    {
        _villager = villager;
    }

    public override Node.Status Start()
    {
        NPCPoolManager.Instance.ReturnVillagerToPool(_villager);

        if (_villager.gameObject.activeSelf == false)
            return Node.Status.Success;
        else
            return Node.Status.Failure;
    }

    public override Node.Status Update()
    {
        return Node.Status.Success;
    }
}
