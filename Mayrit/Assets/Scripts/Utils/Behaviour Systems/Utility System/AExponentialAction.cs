using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for actions that have an exponential decision factor (float).
/// </summary>
public abstract class AExponentialAction : AAction<float>
{
    #region PROPERTIES
    readonly bool _inverted;
    #endregion

    #region CONSTRUCTOR
    protected AExponentialAction(string name, UtilitySystem utilitySystem, bool inverted = false)
    : base(name, utilitySystem)
    {
        _inverted = inverted;
    }
    #endregion

    #region INHERITED METHODS
    protected override float CalculateUtility()
    {
        _utility = (float)Math.Pow(DecisionFactor, 2); // Exponential function

        if (_inverted)
            _utility = 1f - _utility; // Inverted exponential function

        //Debug.Log(name + " utility: " + utility);
        return _utility;
    }
    #endregion
}