using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TownManager : Singleton<TownManager>
{
    #region EDITOR PROPERTIES
    [Header("Town Stats")]
    public int _population;

    [Header("Places of Interest")]
    public List<Building> _sanctuaries;
    public List<Market> _markets;
    #endregion

    #region INTERNAL PROPERTIES    
    /// <summary>
    /// Event fired when population changes. Provides the new population value.
    /// </summary>
    public event Action<int> OnPopulationChanged;
    readonly List<House> _houses = new();
    readonly List<Workplace> _workplaces = new();
    #endregion

    #region MONOBEHAVIOUR
    void Start()
    {
        // Subscribe to milestone changes to update population accordingly
        ProgressManager.Instance.OnMilestoneChanged += OnMilestoneChanged;
    }
    void OnDestroy()
    {
        // Unsubscribe from milestone changes
        ProgressManager.ExistingInstance.OnMilestoneChanged -= OnMilestoneChanged;
    }
    #endregion

    #region PUBLIC METHODS  
    /// <summary>
    /// Registers a house in the town and updates population accordingly.
    /// </summary>
    public void RegisterHouse(House house)
    {
        if (house == null) return;

        if (!_houses.Contains(house))
        {
            _houses.Add(house);
            UpdatePopulation(house._capacity);
        }
    }

    /// <summary>
    /// Unregisters a house from the town and updates population accordingly.
    /// </summary>
    public void UnregisterHouse(House house)
    {
        if (house == null) return;

        if (_houses.Contains(house))
        {
            _houses.Remove(house);
            UpdatePopulation(-house._capacity);
        }
    }

    /// <summary>
    /// Registers a workplace in the town.
    /// </summary>
    public void RegisterWorkplace(Workplace workplace)
    {
        if (workplace == null) return;

        if (!_workplaces.Contains(workplace))
        {
            _workplaces.Add(workplace);
        }
    }

    /// <summary>   
    /// Unregisters a workplace from the town.
    /// </summary>
    public void UnregisterWorkplace(Workplace workplace)
    {
        if (workplace == null) return;

        if (_workplaces.Contains(workplace))
        {
            _workplaces.Remove(workplace);
        }
    }

    /// <returns>Random registered house with capacity for a new resident.</returns>
    public House GetRandomHouseWithFreeSpace()
    {
        return GetRandomHouseWithFreeCapacity(null);
    }

    /// <returns>Random registered house with capacity for a new resident. Excluding given house.</returns>
    public House GetRandomHouseWithFreeCapacity(House excludedHouse)
    {
        if (_houses == null || _houses.Count == 0)
            return null;

        // Build a list of candidate houses with available slots
        List<House> housesWithFreeSlots = new();

        // Check every house
        foreach (var house in _houses)
        {
            if (house == excludedHouse) continue;
            if (!house.AtMaxCapacity)
                housesWithFreeSlots.Add(house);
        }

        // No houses with free slots found
        if (housesWithFreeSlots.Count == 0)
            return null;

        // Return a random house from the candidates
        return housesWithFreeSlots[UnityEngine.Random.Range(0, housesWithFreeSlots.Count)];
    }

    private Workplace GetRandomWorkplaceWithFreeCapacity(Workplace excludedWorkplace)
    {
        if (_workplaces == null || _workplaces.Count == 0)
            return null;

        // Build a list of candidate houses with available slots
        List<Workplace> workPlacesWithCapacity = new();

        // Check every house
        foreach (var workplace in _workplaces)
        {
            if (workplace == excludedWorkplace) continue;
            if (!workplace.AtMaxCapacity)
                workPlacesWithCapacity.Add(workplace);
        }

        // No houses with free slots found
        if (workPlacesWithCapacity.Count == 0)
            return null;

        // Return a random house from the candidates
        return workPlacesWithCapacity[UnityEngine.Random.Range(0, workPlacesWithCapacity.Count)];
    }

    public Building GetRandomWorkplaceBuilding()
    {
        // TODO
        return _workplaces[UnityEngine.Random.Range(0, _houses.Count)];
    }

    public Spot GetMarketSpot()
    {
        // TODO
        return _markets[UnityEngine.Random.Range(0, _markets.Count)].GetRandomEntranceSpot();
    }
    #endregion

    #region PRIVATE METHODS
    void UpdatePopulation(int householdSize)
    {
        _population += householdSize;
    }

    void OnMilestoneChanged(ProgressManager.Milestone milestone)
    {
        OnPopulationChanged?.Invoke(_population);
    }
    #endregion

    /// <summary>
    /// Attempts to reassign residents from a destroyed house to other houses with free capacity.
    /// If a resident cannot be reassigned, it will be returned to the NPC pool and population decremented.
    /// </summary>
    public void ReassignResidents(House previousHouse, List<Villager> residents)
    {
        if (residents == null || residents.Count == 0) return;

        // Process each villager independently
        foreach (var villager in residents)
        {
            if (villager == null) continue;

            try
            {
                House randomHouse = GetRandomHouseWithFreeCapacity(previousHouse);

                // A different house with free capacity was found
                if (randomHouse != null)
                {
                    // Reassign
                    villager.AssignHome(randomHouse);
                    randomHouse.AssignNewResident(villager);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"ReassignResidents: exception while reassigning {villager.name}: {ex}");

                try
                {
                    NPCPoolManager.Instance.ReturnVillagerToPool(villager);
                }
                catch { }
            }
        }
    }

    public void ReassignEmployees(Workplace previousWorkplace, List<Villager> employees)
    {
        if (employees == null || employees.Count == 0) return;

        // Process each villager independently
        foreach (var villager in employees)
        {
            if (villager == null) continue;

            try
            {
                Workplace randomWorkplace = GetRandomWorkplaceWithFreeCapacity(previousWorkplace);

                // A different workplace with free capacity was found
                if (randomWorkplace != null)
                {
                    // Reassign
                    villager.AssignWorkplace(randomWorkplace);
                    randomWorkplace.AssignNewEmployee(villager);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"ReassignResidents: exception while reassigning {villager.name}: {ex}");

                try
                {
                    NPCPoolManager.Instance.ReturnVillagerToPool(villager);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// Finds and returns the sanctuary Building nearest to the provided home.
    /// </summary>
    /// <param name="home">The house used as the reference point for distance calculations.</param>
    /// <returns>The nearest sanctuary Building, or null if none available.</returns>
    public Building GetNearestSanctuary(House home)
    {
        // Validate inputs: no sanctuaries configured or invalid home -> nothing to do
        if (_sanctuaries == null || _sanctuaries.Count == 0 || home == null)
            return null;

        Building nearestSanctuary = null;
        float nearestDistanceSqr = float.MaxValue;
        Vector3 homePosition = home.transform.position;

        foreach (var sanctuary in _sanctuaries)
        {
            if (sanctuary == null)
                continue; // skip null entries in the list

            // Use squared magnitude to avoid the cost of sqrt when comparing distances
            float distanceSqr = (sanctuary.transform.position - homePosition).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestSanctuary = sanctuary;
            }
        }

        return nearestSanctuary;
    }


}
