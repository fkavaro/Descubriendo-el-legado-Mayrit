using UnityEngine;
using UnityEngine.AI;

public class Villager : ANPC<BehaviourTree>
{
    #region EDIROR PROPERTIES
    [Header("Villager Properties")]
    public House _home;
    #endregion

    #region NODES
    BehaviourTree _villagerBT;
    InfiniteLoopNode infiniteLoop;
    SequenceNode behaviourSequence,
        annoyAlchemistSequence,
        annoySorcererSequence,
        restSequence;
    SelectorNode annoyWorkerSelector;
    LeafNode isAlchemistNearLeaf,
        annoyAlchemistLeaf,
        isSorcererNearLeaf,
        annoySorcererLeaf,
        walkAroundLeaf,
        isEnergyLowConditionLeaf,
        restLeaf,
        canAnnoyAlchemistLeaf,
        canAnnoySorcererLeaf;
    SuccederNode successerAnnoyWorker;
    #endregion

    #region STRATEGIES
    ConditionStrategy isEnergyLowStrategy;
    AtHome_VillagerStrategy restStrategy;
    Working_VillagerStrategy workingStrategy;
    Shopping_VillagerStrategy shopSorcererStrategy;
    Praying_VillagerStrategy prayingStrategy;
    #endregion

    #region INHERITED
    public override BehaviourTree InitializeBehaviourSystem()
    {
        // Strategies
        isEnergyLowStrategy = new(this, IsEnergyLow);
        restStrategy = new(this);
        workingStrategy = new(this);
        shopSorcererStrategy = new(this);
        prayingStrategy = new(this);

        // canAnnoyAlchemistStrategy = new(this, CanAnnoyAlchemist);
        // canAnnoySorcererStrategy = new(this, CanAnnoySorcerer);

        // Annoy alchemist sequence
        // canAnnoyAlchemistLeaf = new(this, "CanAnnoyAlchemist", canAnnoyAlchemistStrategy);
        // isAlchemistNearLeaf = new(this, "IsAlchemistNear", isAlchemistNearStrategy);
        // annoyAlchemistLeaf = new(this, "Annoying Alchemist", annoyingAlchemistStrategy);
        // annoyAlchemistSequence = new(this);
        // annoyAlchemistSequence.AddChild(isAlchemistNearLeaf);
        // annoyAlchemistSequence.AddChild(canAnnoyAlchemistLeaf);
        // annoyAlchemistSequence.AddChild(annoyAlchemistLeaf);

        // Annoy sorcerer sequence
        // canAnnoySorcererLeaf = new(this, "CanAnnoySorcerer", canAnnoySorcererStrategy);
        // isSorcererNearLeaf = new(this, "IsSorcererNear", isSorcererNearStrategy);
        // annoySorcererLeaf = new(this, "Annoying Sorcerer", annoyingSorcererStrategy);
        // annoySorcererSequence = new(this);
        // annoySorcererSequence.AddChild(isSorcererNearLeaf);
        // annoySorcererSequence.AddChild(canAnnoySorcererLeaf);
        // annoySorcererSequence.AddChild(annoySorcererLeaf);

        // Annoy worker selector
        // annoyWorkerSelector = new(this);
        // annoyWorkerSelector.AddChild(annoyAlchemistSequence);
        // annoyWorkerSelector.AddChild(annoySorcererSequence);

        // Selector succeder
        // successerAnnoyWorker = new(this);
        // successerAnnoyWorker.AddChild(annoyWorkerSelector);

        // Walk around
        // walkAroundLeaf = new(this, "Walking around", randomDestinationStrategy);

        // Rest sequence
        isEnergyLowConditionLeaf = new(this, "IsEnergyLow", isEnergyLowStrategy);
        restLeaf = new(this, "Resting", restStrategy);
        restSequence = new(this);
        restSequence.AddChild(isEnergyLowConditionLeaf);
        restSequence.AddChild(restLeaf);

        // Behaviour sequence
        behaviourSequence = new(this);
        behaviourSequence.AddChild(successerAnnoyWorker);
        behaviourSequence.AddChild(walkAroundLeaf);
        behaviourSequence.AddChild(restSequence);

        infiniteLoop = new(this, behaviourSequence);
        _villagerBT = new(this, infiniteLoop);

        return _villagerBT;
    }
    #endregion

    #region PUBLIC METHODS
    public void AssignHome(House home)
    {
        _home = home;
    }

    public void OnReleasedFromPool()
    {
        _home.RemoveResident(this);
        _home = null;
    }

    public void ReturnHomeAndRelease()
    {
        // // Optionally move to a home entrance spot before release
        // if (_house != null)
        // {
        //     Spot spawnSpot = _house.GetRandomEntranceSpot();
        //     if (spawnSpot != null)
        //     {
        //         transform.position = spawnSpot.transform.position;
        //         if (spawnSpot._isRotationFixed)
        //             ForceRotation(spawnSpot.DirectionVector);
        //     }
        //     else
        //     {
        //         transform.position = _house.transform.position;
        //     }
        // }

        // Return to pool
        NPCPoolManager.Instance.ReturnVillagerToPool(this);
    }
    #endregion
}
