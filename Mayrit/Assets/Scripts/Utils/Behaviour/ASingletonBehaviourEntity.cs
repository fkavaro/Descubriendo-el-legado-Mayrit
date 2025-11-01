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
            lock (_instanceLock) // Ensures that only one thread at a time can execute this block.
            {
                if (_instance == null)// If no instance exists, try to find one.
                {
                    _instance = FindFirstObjectByType<M>(); // Searches for an existing instance in the scene.

                    if (_instance == null) // If still null, create a new instance.
                    {
                        GameObject singletonObj = new(typeof(M).Name); // Creates a new GameObject named after the type.
                        _instance = singletonObj.AddComponent<M>(); // Adds the singleton component to the new GameObject.
                    }
                }
                return _instance;
            }
        }
    }
    #endregion

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        // Enforce singleton instance for this closed generic type
        lock (_instanceLock)
        {
            if (_instance == null) // If no instance exists, set this as the instance.
            {
                _instance = this as M;
                if (_instance == null)
                    Debug.LogWarning($"ASingletonBehaviourEntity: Awake couldn't cast 'this' to {typeof(M).Name}.");

                if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject); // Prevents the instance from being destroyed when loading new scenes.
            }
            else if (_instance != this) // If an instance already exists and it's not this one, destroy this object.
            {
                Debug.LogWarning($"Duplicate singleton of {typeof(M).Name} detected. Destroying duplicate.");
                DestroyImmediate(gameObject);
                return;
            }
        }

        base.Awake(); // ABehaviourEntity
    }
    #endregion
}
