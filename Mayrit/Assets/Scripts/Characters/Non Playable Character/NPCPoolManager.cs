using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Pool manager for NPCs (Non-Player Characters).
/// </summary>
public class NPCPoolManager : MonoBehaviour
{
    #region PUBLIC PROPERTIES
    public ObjectPool<ANPC> _npcsPool;

    [Header("NPCs pool")]
    [Tooltip("All npc models to be spawned randomly")]
    public GameObject[] _npcsPrefabs;
    [Tooltip("Maximum number of npcs at once")]
    public int _maxNpcs = 10;
    public int _minSecondsForNewNPC = 2,
        _maxSecondsForNewNPC = 15;

    public Transform entrancePosition;
    #endregion

    #region PRIVATE PROPERTIES
    float _lastSpawnTime = 0f;
    #endregion

    #region EXECUTION METHODS
    void Awake()
    {
        // Pool creation
        _npcsPool = new ObjectPool<ANPC>(
            createFunc: CreateNPC,
            actionOnGet: GetNPC,
            actionOnRelease: ReleaseNPC,
            actionOnDestroy: (npc) => Destroy(npc.gameObject),
            maxSize: _maxNpcs
        );
    }

    // Update is called once per frame
    void Update()
    {
        // npcs keep coming if there's room for them
        if (Time.time >= _lastSpawnTime && _npcsPool.CountActive < _maxNpcs)
        {
            _lastSpawnTime = Time.time + UnityEngine.Random.Range(_minSecondsForNewNPC, _maxSecondsForNewNPC);
            _npcsPool.Get();
        }
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Creation method for npcs pool: instantiates a random npc prefab.
    /// </summary>
    /// <returns>Instantiated npc.</returns>
    ANPC CreateNPC()
    {
        ANPC npc = Instantiate(
            _npcsPrefabs[UnityEngine.Random.Range(0, _npcsPrefabs.Length)], // Random model
            entrancePosition.position, // TODO: npc's house entrance spot position
            Quaternion.identity, // TODO: house entrance spot rotation
            transform) // TODO: each npc parent should be its house
        .GetComponent<ANPC>();
        return npc;
    }

    /// <summary>
    /// Get method for npcs pool: resets npc position and behaviour.
    /// </summary>>
    void GetNPC(ANPC npc)
    {
        npc.transform.position = entrancePosition.position; // TODO: npc's house entrance spot position
        npc._animationController.ChangeAnimationTo(npc._animationController._walkAnim);
        npc.gameObject.SetActive(true);
        npc.DecisionSystem.Reset();
    }

    /// <summary>
    /// Release method for npcs pool: deactivates npc gameobject.
    /// </summary>
    void ReleaseNPC(ANPC npc)
    {
        npc.gameObject.SetActive(false);
    }
    #endregion
}
