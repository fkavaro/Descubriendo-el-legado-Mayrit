using System;
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
    public int _minSecondsForNewVillager = 2,
        _maxSecondsForNewVillager = 15;

    public Transform entrancePosition;
    #endregion

    #region PRIVATE PROPERTIES
    float _lastSpawnTime = 0f;
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
    }

    // Update is called once per frame
    void Update()
    {
        // Villagers keep coming if there's room for them
        if (Time.time >= _lastSpawnTime && _villagerPool.CountActive < _maxVillagers)
        {
            _lastSpawnTime = Time.time + UnityEngine.Random.Range(_minSecondsForNewVillager, _maxSecondsForNewVillager);
            _villagerPool.Get();
        }
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Creation method for villagers pool: instantiates a random villager prefab.
    /// </summary>
    /// <returns>Instantiated villager.</returns>
    Villager CreateVillager()
    {
        Villager villager = Instantiate(
            _villagerPrefabs[UnityEngine.Random.Range(0, _villagerPrefabs.Length)], // Random model
            entrancePosition.position, // TODO: villager's house entrance spot position
            Quaternion.identity, // TODO: house entrance spot rotation
            transform) // TODO: each villager parent should be its house
        .GetComponent<Villager>();
        return villager;
    }

    /// <summary>
    /// Get method for villagers pool: resets villager position and behaviour.
    /// </summary>>
    void GetVillager(Villager villager)
    {
        villager.transform.position = entrancePosition.position; // TODO: villager's house entrance spot position
        villager._animationController.ChangeAnimationTo(villager._animationController._walkAnim);
        villager.gameObject.SetActive(true);
        villager.BehaviourSystem.Reset();
    }

    /// <summary>
    /// Release method for villagers pool: deactivates villager gameobject.
    /// </summary>
    void ReleaseVillager(Villager villager)
    {
        villager.gameObject.SetActive(false);
    }
    #endregion
}
