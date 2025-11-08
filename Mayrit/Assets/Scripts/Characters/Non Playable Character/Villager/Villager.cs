using System;
using UnityEngine;
using UnityEngine.AI;

public class Villager : ANPC<BehaviourTree>
{
    #region EDIROR PROPERTIES
    [Header("Villager Properties")]
    public House _home;
    #endregion

    #region PROPERTIES
    BehaviourTree _villagerBT;
    #endregion

    #region INHERITED
    public override BehaviourTree InitializeBehaviourSystem()
    {
        // Interact strategies
        ConditionStrategy isInStreet = new(this, IsInStreet);
        ConditionStrategy isOtherNearby = new(this, IsOtherNearby);
        ConditionStrategy isEnoughSinceLastInteraction = new(this, IsEnoughTimeSinceLastInteraction);
        InteractStrategy interactStrategy = new(this);

        // Routine strategies
        GoToDestinationStrategy goToMosque = new(this);
        Praying_VillagerStrategy prayingStrategy = new(this);
        GoToDestinationStrategy goToWork = new(this);
        Working_VillagerStrategy workingStrategy = new(this);
        GoToDestinationStrategy goToShop = new(this);
        Shopping_VillagerStrategy shoppingStrategy = new(this);
        GoToDestinationStrategy goHome = new(this);
        AtHome_VillagerStrategy restStrategy = new(this);

        // Interact sequence
        SequenceNode interactSequence = new(this);

        LeafNode isInStreetLeaf = new(this, "IsInStreet", isInStreet);
        LeafNode isOtherNearbyLeaf = new(this, "IsOtherNearby", isOtherNearby);
        LeafNode isEnoughSinceLastInteractionLeaf = new(this, "IsEnoughSinceLastInteraction", isEnoughSinceLastInteraction);
        LeafNode talkLeaf = new(this, "Talking", interactStrategy);

        interactSequence.AddChild(isInStreetLeaf);
        interactSequence.AddChild(isOtherNearbyLeaf);
        interactSequence.AddChild(isEnoughSinceLastInteractionLeaf);
        interactSequence.AddChild(talkLeaf);

        // Routine sequence
        SequenceNode routineSequence = new(this);
        SequenceNode prayingSequence = new(this);
        SequenceNode workingSequence = new(this);
        SequenceNode shoppingSequence = new(this);
        SequenceNode atHomeSequence = new(this);

        LeafNode goToMosqueLeaf = new(this, "GoingToMosque", goToMosque);
        LeafNode prayLeaf = new(this, "Praying", prayingStrategy);
        prayingSequence.AddChild(goToMosqueLeaf);
        prayingSequence.AddChild(prayLeaf);
        LeafNode goToWorkLeaf = new(this, "GoingToWork", goToWork);
        LeafNode workLeaf = new(this, "Working", workingStrategy);
        workingSequence.AddChild(goToWorkLeaf);
        workingSequence.AddChild(workLeaf);
        LeafNode goToShopLeaf = new(this, "GoingToShop", goToShop);
        LeafNode shopLeaf = new(this, "Shopping", shoppingStrategy);
        shoppingSequence.AddChild(goToShopLeaf);
        shoppingSequence.AddChild(shopLeaf);
        LeafNode goHomeLeaf = new(this, "GoingHome", goHome);
        LeafNode restLeaf = new(this, "Resting", restStrategy);
        atHomeSequence.AddChild(goHomeLeaf);
        atHomeSequence.AddChild(restLeaf);

        routineSequence.AddChild(prayingSequence);
        routineSequence.AddChild(workingSequence);
        routineSequence.AddChild(shoppingSequence);
        routineSequence.AddChild(atHomeSequence);

        // Behaviour sequence
        SelectorNode behaviourSelector = new(this);
        behaviourSelector.AddChild(interactSequence);
        behaviourSelector.AddChild(routineSequence);

        InfiniteLoopNode infiniteLoop = new(this, behaviourSelector);
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
    #endregion

    #region PRIVATE METHODS
    private bool IsEnoughTimeSinceLastInteraction()
    {
        throw new NotImplementedException();
    }

    private bool IsOtherNearby()
    {
        throw new NotImplementedException();
    }

    private bool IsInStreet()
    {
        throw new NotImplementedException();
    }
    #endregion
}
