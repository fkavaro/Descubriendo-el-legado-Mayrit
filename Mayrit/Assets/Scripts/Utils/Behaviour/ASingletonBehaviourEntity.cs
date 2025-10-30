using UnityEngine;

public abstract class ASingletonBehaviourEntity<M, T> : Singleton<M>, IBehaviourEntity<T>
where T : ABehaviourSystem
where M : MonoBehaviour
{
    #region PROPERTIES
    [Header("Behaviour settings")]
    /// <summary>
    /// Whether to show debug messages in the console or not
    /// </summary>
    public bool _debugMode;
    /// <summary>
    /// Whether to update next frame of the system or not
    /// </summary>
    public bool _isExecutionPaused;
    #endregion

    #region INTERFACE IMPLEMENTATION
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
    protected override void Awake()
    {
        base.Awake();

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
