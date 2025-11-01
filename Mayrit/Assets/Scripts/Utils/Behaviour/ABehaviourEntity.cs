using UnityEngine;

public abstract class ABehaviourEntity<T> : MonoBehaviour, IBehaviourEntityGeneric<T>
where T : ABehaviourSystem
{
    #region EDITOR PROPERTIES
    [Header("Behaviour System settings")]
    [Tooltip("Whether to show debug messages in the console or not")]
    public bool _debugMode;
    [Tooltip("Whether to pause the execution of the behaviour system or not")]
    public bool _isExecutionPaused;
    #endregion

    #region INTERFACE IMPLEMENTATION
    public GameObject GO => gameObject;
    public bool DebugMode
    {
        get => _debugMode;
        set => BehaviourSystem.DebugMode = value;
    }
    public bool IsExecutionPaused
    {
        get => _isExecutionPaused;
        set => BehaviourSystem.IsExecutionPaused = value;
    }

    public abstract void InitializeBehaviour();
    public abstract T BehaviourSystem { get; }
    #endregion

    #region MONOBEHAVIOUR: DERIVED TO BEHAVIOUR SYSTEM
    protected virtual void Awake()
    {
        InitializeBehaviour();
        BehaviourSystem.Awake();
    }

    protected virtual void Start()
    {
        BehaviourSystem.Start();
    }

    protected virtual void Update()
    {
        BehaviourSystem.Update();
    }

    protected virtual void LateUpdate()
    {
        BehaviourSystem.LateUpdate();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        BehaviourSystem.OnCollisionEnter(collision);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        BehaviourSystem.OnCollisionStay(collision);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        BehaviourSystem.OnCollisionExit(collision);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        BehaviourSystem.OnTriggerEnter(other);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        BehaviourSystem.OnTriggerStay(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        BehaviourSystem.OnTriggerExit(other);
    }
    #endregion
}
