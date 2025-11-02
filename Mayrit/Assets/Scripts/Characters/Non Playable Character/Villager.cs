using UnityEngine;
using UnityEngine.AI;

public class Villager : ANPC<FiniteStateMachine>
{
    // Reference to the home/building this villager belongs to
    ABuilding _home;

    /// <summary>
    /// Assigns a home building to this villager and registers it as a resident.
    /// </summary>
    public void AssignHome(ABuilding home)
    {
        if (home == null) return;
        _home = home;

        // If the building exposes a residents list (House), add this villager
        if (home is House h)
        {
            if (!h._residents.Contains(this))
                h._residents.Add(this);
        }
        // Reparent to the house for tidy hierarchy (optional)
        try { transform.SetParent(home.transform); } catch { }
    }

    /// <summary>
    /// Called by the pool manager when the villager is being released back to the pool.
    /// Use this to clear references and reset runtime state.
    /// </summary>
    public void OnReleasedFromPool()
    {
        // Remove from home's residents list
        if (_home is House h)
        {
            if (h._residents.Contains(this))
                h._residents.Remove(this);
        }

        _home = null;

        // Reset parent to pool manager for cleanliness
        try { transform.SetParent(NPCPoolManager.Instance.transform); } catch { }
    }

    /// <summary>
    /// Ask the villager to return home and be released to the pool.
    /// This is intended to be called by behaviour code when work is finished.
    /// </summary>
    public void ReturnHomeAndRelease()
    {
        // Optionally move to a home entrance spot before release
        if (_home != null)
        {
            Spot spawnSpot = _home.GetRandomEntranceSpot();
            if (spawnSpot != null)
            {
                transform.position = spawnSpot.transform.position;
                if (spawnSpot._isRotationFixed)
                    ForceRotation(spawnSpot.DirectionVector);
            }
            else
            {
                transform.position = _home.transform.position;
            }
        }

        // Return to pool
        NPCPoolManager.Instance.ReturnVillagerToPool(this);
    }

    public override FiniteStateMachine InitializeBehaviourSystem()
    {
        return new FiniteStateMachine(this);
    }
}
