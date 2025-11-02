using System.Collections.Generic;
using UnityEngine;

public class House : ABuilding
{
    #region EDITOR PROPERTIES
    public int _householdSize = 1;
    public List<Villager> _residents = new();
    #endregion

    #region MONOBEHAVIOUR
    // When enabled, increase town population
    public void OnEnable()
    {
        // Register this house and increase town population
        TownManager.Instance.RegisterHouse(this);
        TownManager.Instance.UpdatePopulation(_householdSize);

        // Spawn initial residents for this house (one villager per household slot).
        NPCPoolManager.Instance.SpawnVillagersForHouseBatched(this, _householdSize);
    }

    // When disabled, decrease town population
    public void OnDisable()
    {
        TownManager.Instance.UnregisterHouse(this);

        // Ask TownManager to reassign residents centrally
        // (it will release those that cannot be reassigned and adjust population)
        var residentsCopy = new List<Villager>(_residents);
        TownManager.Instance.ReassignResidents(this, residentsCopy);

        // Clear this house's residents list
        _residents.Clear();
    }
    #endregion
}
