using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for actions in the Utility System.
/// </summary>
public abstract class AAction<TFactor> : IAction
{
    #region PROPERTIES
    public string ActionName => _actionName;
    public float Utility => CalculateUtility();

    protected string _actionName;
    protected float _utility;
    protected UtilitySystem _utilitySystem;
    protected TFactor DecisionFactor => SetDecisionFactor();
    #endregion

    #region CONSTRUCTOR
    public AAction(string actionName, UtilitySystem utilitySystem)
    {
        _actionName = actionName;
        _utilitySystem = utilitySystem;
        utilitySystem.AddAction(this);
    }
    #endregion

    #region TO BE IMPLEMENTED METHODS
    protected abstract TFactor SetDecisionFactor();
    protected abstract float CalculateUtility();
    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
    public virtual void Reset() { }

    public virtual string DebugDecision()
    {
        return ActionName;
    }
    #endregion
}
