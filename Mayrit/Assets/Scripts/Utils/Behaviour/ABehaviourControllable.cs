using UnityEngine;

[RequireComponent(typeof(BehaviourController))]
public abstract class ABehaviourControllable : MonoBehaviour, IBehaviourControllable
{
    #region PROPERTIES
    public string Name => gameObject.name;
    public BehaviourController BehaviourController => _behaviourController;

    BehaviourController _behaviourController;
    #endregion

    #region METHODS
    public abstract ADecisionSystem CreateDecisionSystem();

    protected virtual void Awake()
    {
        _behaviourController = GetComponent<BehaviourController>();
        _behaviourController._decisionSystem = CreateDecisionSystem();
    }
    #endregion

}
