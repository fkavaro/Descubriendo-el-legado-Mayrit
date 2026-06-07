public class CollectiblesTracker : AObjectivesTracker<CollectiblesTracker, Collectible, CollectibleSO>
{
    protected override void OnObjectiveReachedAction(Collectible reachedObj)
    {
        reachedObj.Complete();
        reachedObj.UpdateVFX();
    }
}