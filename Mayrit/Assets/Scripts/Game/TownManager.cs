using System;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : Singleton<TownManager>
{
    #region EDITOR PROPERTIES
    [Header("Town Stats")]
    public int _population;

    [Header("Debug")]
    [Tooltip("Show reassignment debug overlay and logs")]
    public bool _debugReassignment = false;
    #endregion

    #region INTERNAL PROPERTIES    
    /// <summary>
    /// Event fired when population changes. Provides the new population value.
    /// </summary>
    public event Action<int> OnPopulationChanged;

    ReassignmentOutcome _lastReassignment = null;

    // Registered houses in the town
    private List<House> _houses = new List<House>();
    #endregion

    #region PUBLIC METHODS  
    public void UpdatePopulation(int householdSize)
    {
        _population += householdSize;
        OnPopulationChanged?.Invoke(_population);
    }

    public void RegisterHouse(House house)
    {
        if (house == null) return;
        if (!_houses.Contains(house))
            _houses.Add(house);
    }

    public void UnregisterHouse(House house)
    {
        if (house == null) return;
        if (_houses.Contains(house))
            _houses.Remove(house);
    }

    /// <summary>
    /// Attempts to reassign residents from a destroyed house to other houses with free capacity.
    /// If a resident cannot be reassigned, it will be returned to the NPC pool and population decremented.
    /// </summary>
    public void ReassignResidents(House fromHouse, List<Villager> residents)
    {
        if (residents == null || residents.Count == 0) return;

        // Build candidate list: all registered houses that have at least one free slot.
        // Exclude the source (destroyed) house so we don't reassign back to it.
        var candidates = new List<House>();
        for (int i = 0; i < _houses.Count; i++)
        {
            var h = _houses[i];
            if (h == null || h == fromHouse) continue;
            if (h._residents.Count < h._householdSize)
                candidates.Add(h);
        }

        int releasedCount = 0;

        // Prepare a ReassignmentOutcome object to record details useful for debugging/overlay.
        // - fromHouseName: name of the house we are reassigning from (might be null if unknown)
        // - timestamp: time of reassignment
        // - attempted: how many residents we attempted to reassign
        // - assignments: textual list of individual outcomes (assigned to which house or released)
        // - released: number of villagers returned to the pool
        var outcome = new ReassignmentOutcome
        {
            fromHouseName = fromHouse != null ? fromHouse.name : "(null)",
            timestamp = Time.time,
            attempted = residents.Count,
            assignments = new List<string>(),
            released = 0
        };

        // Shortcut to the NPC pool manager to return villagers we cannot place.
        var pool = NPCPoolManager.Instance;

        // Process each villager independently. We handle exceptions per-villager so a single
        // failure does not abort reassignment of others.
        for (int i = 0; i < residents.Count; i++)
        {
            var v = residents[i];
            if (v == null) continue;

            try
            {
                // If there are no candidate houses left, we must release the villager back to the pool.
                // This decrements town population later (below) for all released villagers.
                if (candidates.Count == 0)
                {
                    pool?.ReturnVillagerToPool(v);
                    releasedCount++;
                    outcome.released++;
                    outcome.assignments.Add(v.gameObject.name + " -> (released)");
                    if (_debugReassignment) Debug.Log($"ReassignResidents: released {v.gameObject.name} (no candidate houses)");
                    continue;
                }

                // Choose a candidate at random to distribute load across houses.
                int idx = UnityEngine.Random.Range(0, candidates.Count);
                var best = candidates[idx];

                // Defensive check: if the selected house is unexpectedly null, release the villager.
                if (best == null)
                {
                    pool?.ReturnVillagerToPool(v);
                    releasedCount++;
                    outcome.released++;
                    outcome.assignments.Add(v.gameObject.name + " -> (released)");
                    if (_debugReassignment) Debug.Log($"ReassignResidents: released {v.gameObject.name} (selected null)");
                    continue;
                }

                // Perform the reassignment:
                // - Update the villager's home reference so house/resident lists stay consistent.
                // - Move the villager to an entrance/spawn spot if available (for visual correctness).
                // - Optionally fix rotation if the spawn spot requires it.
                v.AssignHome(best);
                var spawnSpot = best.GetRandomEntranceSpot();
                if (spawnSpot != null)
                {
                    // Place the villager at the spawn spot and apply rotation if the spot enforces it.
                    v.transform.position = spawnSpot.transform.position;
                    if (spawnSpot._isRotationFixed)
                        v.ForceRotation(spawnSpot.DirectionVector);
                }
                else
                {
                    // Fallback: place at the house origin if no spawn spot exists.
                    v.transform.position = best.transform.position;
                }

                outcome.assignments.Add(v.gameObject.name + " -> " + best.name);
                if (_debugReassignment) Debug.Log($"ReassignResidents: assigned {v.gameObject.name} -> {best.name}");

                // If this house reached capacity after the assignment, remove it from the candidate pool
                // so subsequent villagers aren't assigned to an already-full house.
                if (best._residents.Count >= best._householdSize)
                    candidates.RemoveAt(idx);
            }
            catch (Exception ex)
            {
                // Log the error for diagnostics and make a best-effort attempt to release the villager
                // so we don't leak a simulation entity into an invalid state.
                Debug.LogError($"ReassignResidents: exception while reassigning {v?.name}: {ex}");
                try { pool?.ReturnVillagerToPool(v); } catch { /* swallow secondary errors */ }
                releasedCount++;
                outcome.released++;
                outcome.assignments.Add(v.gameObject.name + " -> (released on error)");
            }
        }

        // If any villagers were released to the pool (i.e., lost homes), decrement the town population.
        if (releasedCount > 0)
            UpdatePopulation(-releasedCount);

        // Save the outcome so the debug overlay can display what happened during this reassignment.
        _lastReassignment = outcome;
    }

    // /// <summary>
    // /// Returns a random registered house excluding the given one (or null if none available).
    // /// </summary>
    // public House GetRandomHouseExcept(House exclude)
    // {
    //     if (_houses == null || _houses.Count == 0) return null;
    //     if (_houses.Count == 1 && _houses.Contains(exclude)) return null;

    //     int count = _houses.Count;
    //     if (count == 1)
    //         return _houses[0] == exclude ? null : _houses[0];

    //     // Try up to 'count' times to pick a random house that's not the excluded one.
    //     for (int i = 0; i < count; i++)
    //     {
    //         var candidate = _houses[UnityEngine.Random.Range(0, count)];
    //         if (candidate != exclude) return candidate;
    //     }

    //     // Fallback: no suitable candidate found
    //     return null;
    // }

    // /// <summary>
    // /// Returns a random registered house that has free capacity (residents < householdSize),
    // /// excluding the provided house. Returns null if none available.
    // /// </summary>
    // public House GetRandomHouseWithFreeSlotExcept(House exclude)
    // {
    //     if (_houses == null || _houses.Count == 0) return null;

    //     // Build a list of candidate houses with available slots
    //     var candidates = new List<House>();
    //     for (int i = 0; i < _houses.Count; i++)
    //     {
    //         var h = _houses[i];
    //         if (h == null || h == exclude) continue;
    //         if (h._residents.Count < h._householdSize)
    //             candidates.Add(h);
    //     }

    //     if (candidates.Count == 0) return null;

    //     return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    // }
    #endregion

    void OnGUI()
    {
        if (!_debugReassignment) return;

        const int margin = 10;
        int x = margin;
        int y = margin;
        int w = 380;
        int h = 20;

        GUI.Box(new Rect(x - 6, y - 6, w + 12, 260), "TownManager Debug");

        GUI.Label(new Rect(x, y, w, h), $"Population: {_population}");
        y += h;
        GUI.Label(new Rect(x, y, w, h), $"Houses registered: {_houses.Count}");
        y += h;

        GUI.Label(new Rect(x, y, w, h), "Houses (name : residents/capacity):");
        y += h;
        foreach (var hObj in _houses)
        {
            if (hObj == null) continue;
            GUI.Label(new Rect(x, y, w, h), $"- {hObj.name} : {hObj._residents.Count}/{hObj._householdSize}");
            y += h;
        }

        y += 6;
        GUI.Label(new Rect(x, y, w, h), "Last reassignment:");
        y += h;
        if (_lastReassignment != null)
        {
            GUI.Label(new Rect(x, y, w, h), $"From: {_lastReassignment.fromHouseName} @ {_lastReassignment.timestamp:F1}");
            y += h;
            GUI.Label(new Rect(x, y, w, h), $"Attempted: {_lastReassignment.attempted}, Released: {_lastReassignment.released}");
            y += h;
            GUI.Label(new Rect(x, y, w, h), "Assignments:");
            y += h;
            foreach (var s in _lastReassignment.assignments)
            {
                GUI.Label(new Rect(x + 8, y, w, h), s);
                y += h;
            }
        }
        else
        {
            GUI.Label(new Rect(x, y, w, h), "(none)");
        }
    }

    class ReassignmentOutcome
    {
        public string fromHouseName;
        public float timestamp;
        public int attempted;
        public int released;
        public List<string> assignments;
    }
}
