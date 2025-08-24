using UnityEngine;

public abstract class ADecisionSystem
{
    public readonly ABehaviourController _controller;
    public readonly IBehaviourControllable _controllable;


    public ADecisionSystem(ABehaviourController controller)
    {
        controller._decisionSystem = this;
        _controller = controller;
        _controllable = controller._controllable;
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