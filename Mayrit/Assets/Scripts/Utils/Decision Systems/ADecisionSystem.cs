using UnityEngine;

/// <summary>
/// Abstract class that defines a decision system for a controllable object.
/// </summary>
public abstract class ADecisionSystem
{
    public readonly IBehaviourControllable _controllable;
    protected bool DebugMode => _controllable.BehaviourController._debugMode;
    public bool IsExecutionPaused => _controllable.BehaviourController._isExecutionPaused;

    // Constructor
    public ADecisionSystem(IBehaviourControllable controllable)
    {
        _controllable = controllable;
        _controllable.BehaviourController._decisionSystem = this;
    }

    protected abstract void DebugDecision();

    public virtual void Awake() { }
    public virtual void Start() { }
    public abstract void Update();
    public virtual void LateUpdate() { }

    public abstract void Reset();

    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }

    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
}