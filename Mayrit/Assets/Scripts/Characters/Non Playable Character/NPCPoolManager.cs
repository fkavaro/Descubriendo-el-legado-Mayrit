using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Pool manager for NPCs (Non-Player Characters).
/// </summary>
public class NPCPoolManager : Singleton<NPCPoolManager>
{
    #region PUBLIC PROPERTIES
    public ObjectPool<Villager> _villagerPool;
    // TODO: soldier pool

    [Header("Villagers pool")]
    [Tooltip("All villager models to be spawned randomly")]
    public GameObject[] _villagerPrefabs;
    [Tooltip("Maximum number of villager at once")]
    public int _maxVillagers = 10;
    [Tooltip("How many villager spawns to perform per frame when batching")]
    public int _spawnPerFrame = 3;
    [Tooltip("How many prefab instantiations to perform per frame when prewarming the pool")]
    public int _prewarmPerFrame = 5;
    [Tooltip("Optional spawn points to use when TownManager requests global spawns")]
    public Transform[] _globalSpawnPoints;
    #endregion

    #region PRIVATE PROPERTIES
    #endregion

    #region EXECUTION METHODS
    protected override void Awake()
    {
        base.Awake();

        // Pool creation
        _villagerPool = new ObjectPool<Villager>(
            createFunc: CreateVillager,
            actionOnGet: GetVillager,
            actionOnRelease: ReleaseVillager,
            actionOnDestroy: (villager) => Destroy(villager.gameObject),
            maxSize: _maxVillagers
        );

        // Subscribe to town population changes (if TownManager is available)
        if (TownManager.Instance != null)
            TownManager.Instance.OnPopulationChanged += OnTownPopulationChanged;
    }

    void OnDestroy()
    {
        if (TownManager.Instance != null)
            TownManager.Instance.OnPopulationChanged -= OnTownPopulationChanged;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Time.time >= _lastSpawnTime && _villagerPool.CountActive < _maxVillagers)
        // {
        //     _lastSpawnTime = Time.time + UnityEngine.Random.Range(_minSecondsForNewVillager, _maxSecondsForNewVillager);
        //     _villagerPool.Get();
        // }
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Creation method for villagers pool: instantiates a random villager prefab.
    /// </summary>
    /// <returns>Instantiated villager.</returns>
    Villager CreateVillager()
    {
        // Instantiate at pool transform; actual spawn position will be set when the villager is taken from the pool
        GameObject prefab = _villagerPrefabs[UnityEngine.Random.Range(0, _villagerPrefabs.Length)];
        Villager villager = Instantiate(prefab, transform.position, Quaternion.identity, transform).GetComponent<Villager>();
        return villager;
    }

    /// <summary>
    /// Get method for villagers pool: resets villager position and behaviour.
    /// </summary>>
    // Active villagers bookkeeping
    List<Villager> _activeVillagers = new List<Villager>();

    void GetVillager(Villager villager)
    {
        // Activation and lightweight reset. Positioning and assignment is handled by SpawnVillagerForHouse
        villager.gameObject.SetActive(true);
        // Attempt a safe behaviour reset if available
        try { villager.BehaviourSystem?.Reset(); } catch { }
        // Reset animations if controller available
        try { villager._animationController.ChangeAnimationTo(villager._animationController._walkAnim); } catch { }
        // Track active
        if (!_activeVillagers.Contains(villager))
            _activeVillagers.Add(villager);
    }

    /// <summary>
    /// Release method for villagers pool: deactivates villager gameobject.
    /// </summary>
    void ReleaseVillager(Villager villager)
    {
        // Let the villager clear references/state before deactivation
        try { villager.OnReleasedFromPool(); } catch { }
        // Track active
        if (_activeVillagers.Contains(villager))
            _activeVillagers.Remove(villager);

        villager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Spawns a villager for the given building. The villager will be positioned at one of the building's entrance spots
    /// and will be assigned to the building's residents list.
    /// </summary>
    public Villager SpawnVillagerForHouse(ABuilding building)
    {
        // Synchronous single spawn (keeps original behavior)
        return GetAndPlaceVillager(building);
    }

    // Internal helper: get a villager from the pool, place it at a building entrance and assign home
    Villager GetAndPlaceVillager(ABuilding building)
    {
        if (_villagerPrefabs == null || _villagerPrefabs.Length == 0) return null;
        if (_villagerPool == null) return null;

        Spot spawnSpot = building.GetRandomEntranceSpot();

        Villager v = _villagerPool.Get();

        if (spawnSpot != null)
        {
            v.transform.position = spawnSpot.transform.position;
            if (spawnSpot._isRotationFixed)
                v.ForceRotation(spawnSpot.DirectionVector);
        }
        else
        {
            v.transform.position = building.transform.position;
        }

        // Assign home and add to residents
        v.AssignHome(building);

        return v;
    }

    /// <summary>
    /// Spawn multiple villagers for a building, spread across frames to avoid spikes.
    /// </summary>
    public void SpawnVillagersForHouseBatched(ABuilding building, int count)
    {
        if (count <= 0 || building == null) return;
        StartCoroutine(SpawnVillagersForHouseCoroutine(building, count));
    }

    IEnumerator SpawnVillagersForHouseCoroutine(ABuilding building, int count)
    {
        int spawned = 0;
        while (spawned < count)
        {
            int batch = Mathf.Min(_spawnPerFrame, count - spawned);
            for (int i = 0; i < batch; i++)
            {
                GetAndPlaceVillager(building);
                spawned++;
            }
            // yield a frame to spread cost
            yield return null;
        }
    }

    /// <summary>
    /// Prewarm the pool by instantiating `amount` villagers and immediately releasing them back to the pool.
    /// This is performed across frames to avoid hitches.
    /// </summary>
    public void PrewarmPool(int amount)
    {
        if (amount <= 0 || _villagerPool == null) return;
        StartCoroutine(PrewarmPoolCoroutine(amount));
    }

    IEnumerator PrewarmPoolCoroutine(int amount)
    {
        int created = 0;
        while (created < amount)
        {
            int batch = Mathf.Min(_prewarmPerFrame, amount - created);
            for (int i = 0; i < batch; i++)
            {
                Villager v = _villagerPool.Get();
                _villagerPool.Release(v);
                created++;
            }
            yield return null;
        }
    }

    // Handle town population change events: spawn or retire villagers to match desired population
    void OnTownPopulationChanged(int newPopulation)
    {
        int desired = Mathf.Clamp(newPopulation, 0, _maxVillagers);
        int active = _activeVillagers.Count;
        int delta = desired - active;

        if (delta > 0)
        {
            // Spawn delta villagers using global spawn points if available
            StartCoroutine(SpawnGlobalVillagersCoroutine(delta));
        }
        else if (delta < 0)
        {
            int toRetire = -delta;
            for (int i = 0; i < toRetire; i++)
            {
                if (_activeVillagers.Count == 0) break;
                Villager v = _activeVillagers[_activeVillagers.Count - 1];
                // Ask villager to go home and release
                try { v.ReturnHomeAndRelease(); } catch { }
            }
        }
    }

    IEnumerator SpawnGlobalVillagersCoroutine(int amount)
    {
        int spawned = 0;
        while (spawned < amount)
        {
            int batch = Mathf.Min(_spawnPerFrame, amount - spawned);
            for (int i = 0; i < batch; i++)
            {
                Villager v = _villagerPool.Get();
                // Place at a random global spawn point if available
                if (_globalSpawnPoints != null && _globalSpawnPoints.Length > 0)
                {
                    Transform s = _globalSpawnPoints[UnityEngine.Random.Range(0, _globalSpawnPoints.Length)];
                    v.transform.position = s.position;
                    v.ForceRotation(s.rotation);
                }
                else
                {
                    v.transform.position = transform.position;
                }
                spawned++;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Public method to return a villager instance back to the pool.
    /// </summary>
    public void ReturnVillagerToPool(Villager villager)
    {
        if (_villagerPool == null || villager == null) return;
        _villagerPool.Release(villager);
    }
    #endregion
}
