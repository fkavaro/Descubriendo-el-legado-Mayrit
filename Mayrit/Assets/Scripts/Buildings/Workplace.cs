using System;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : Building
{
    #region EDITOR PROPERTIES
    public int _capacity = 1;
    [SerializeField] List<Villager> _employees = new();
    public bool AtMaxCapacity => _employees.Count >= _capacity;
    #endregion

    #region MONOBEHAVIOUR
    // When enabled, increase town population
    public void OnEnable()
    {
        // Register this house and update town population
        TownManager.Instance.RegisterWorkplace(this);
    }

    // When disabled, decrease town population
    public void OnDisable()
    {
        // Safely unregister this workplace
        // Use ExistingInstance to avoid creating TownManager during teardown.
        var tm = TownManager.ExistingInstance;
        if (tm != null)
        {
            tm.UnregisterWorkplace(this);

            // There are employees assigned to this workplace: ask TownManager to reassign them
            if (_employees.Count > 0)
            {
                List<Villager> employeesCopy = new(_employees);

                try
                {
                    tm.ReassignEmployees(this, employeesCopy);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"House.OnDisable: ReassignResidents failed: {ex}");
                }

                _employees.Clear();
            }
        }
        // else
        // {
        //     // TownManager is not available (likely during teardown). Return residents to pool if possible, then clear.
        //     var pool = NPCPoolManager.ExistingInstance;
        //     if (_employees.Count > 0)
        //     {
        //         if (pool != null)
        //         {
        //             // Iterate over a snapshot to avoid collection-modified exceptions:
        //             var snapshot = _employees.ToArray();
        //             foreach (var v in snapshot)
        //             {
        //                 try { pool.ReturnVillagerToPool(v); } catch { }
        //             }
        //         }
        //         _employees.Clear();
        //     }
        // }
    }
    #endregion

    #region PUBLIC METHODS
    public bool AssignNewEmployee(Villager villager)
    {
        // Max employees reached or already assigned
        if (_employees.Count >= _capacity || _employees.Contains(villager))
            return false;

        _employees.Add(villager);
        return true;
    }

    public void RemoveEmployee(Villager villager)
    {
        if (_employees.Contains(villager))
            _employees.Remove(villager);
    }
    #endregion
}
