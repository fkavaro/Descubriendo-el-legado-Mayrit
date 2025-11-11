using UnityEngine;

/// <summary>
/// Generic Singleton Behaviour Entity class combining Singleton and BehaviourEntity functionalities.
/// </summary>
/// <typeparam name="M">The type that extends MonoBehaviour for singleton behavior.</typeparam>
/// <typeparam name="T">The type of the behaviour system.</typeparam>
public abstract class ASingletonBehaviourEntity<M, T> : ABehaviourEntity<T>
where T : ABehaviourSystem
where M : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [Header("Singleton")]
    [Tooltip("Whether to persist the singleton instance across scene loads")]
    public bool _dontDestroyOnLoad = false;

    /// <summary>
    /// Static instance and lock for thread-safe singleton access per closed generic type.
    /// </summary>
    static M _instance;
    /// <summary>
    /// Lock object to ensure thread safety when creating the instance.
    /// </summary>
    static readonly object _instanceLock = new();

    /// <summary>
    /// Accessor for the singleton instance. Creates or finds an instance if none exists.
    /// </summary>
    public static M Instance
    {
        get
        {
            lock (_instanceLock)
            {
                // Attempt to find an existing instance in the scene but do NOT create one here.
                if (_instance == null)
                    _instance = FindAnyObjectByType<M>();

                return _instance;
            }
        }
    }

    /// <summary>
    /// Returns the existing instance if one exists, without creating a new GameObject.
    /// Use this when you want to safely query for a singleton during teardown or editor callbacks
    /// and avoid creating a new GameObject.
    /// </summary>
    public static M ExistingInstance => _instance;
    #endregion

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        lock (_instanceLock) // Ensures thread safety when setting the instance.
        {
            if (_instance == null) // If no instance exists, set this as the instance.
            {
                _instance = this as M;
                if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject); // Prevents the instance from being destroyed when loading new scenes.
            }
            else if (_instance != this) // If an instance already exists and it's not this one, destroy this object.
            {
                Debug.LogWarning($"Duplicate Singleton<{typeof(M).Name}> found. Destroying...");
                if (Application.isPlaying) Destroy(gameObject);
                else DestroyImmediate(gameObject);
            }
        }

        base.Awake(); // ABehaviourEntity
    }
    #endregion
}
