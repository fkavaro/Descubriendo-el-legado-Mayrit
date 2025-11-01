using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract class for behaviour decision systems.
/// </summary>
public abstract class ABehaviourSystem
{
    #region PROPERTIES
    public IBehaviourEntity<ABehaviourSystem> _behaviourEntity;
    public GameObject _behaviourEntityGO;

    /// <summary>
    /// Whether to show debug messages in the console or not
    /// </summary>
    public bool _debugMode;
    /// <summary>
    /// Whether to update next frame of the system or not
    /// </summary>
    public bool _isExecutionPaused;

    public bool DebugMode
    {
        get => _debugMode;
        set => _behaviourEntity.DebugMode = value;
    }
    public bool IsExecutionPaused
    {
        get => _isExecutionPaused;
        set => _behaviourEntity.IsExecutionPaused = value;
    }
    #endregion

    #region CONSTRUCTOR
    public ABehaviourSystem(IBehaviourEntity<ABehaviourSystem> entity, GameObject entityGO)
    {
        _behaviourEntity = entity;
        _behaviourEntityGO = entityGO;
    }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    /// <summary>
    /// Debugs the current decision of the system.
    /// </summary>
    protected abstract void DebugDecision();
    /// <summary>
    /// Resets the decision system to its initial state.
    /// </summary>
    public abstract void Reset();
    #endregion

    #region MONOBEHAVIOUR EQUIVALENTS: OPTIONAL
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    #endregion
}