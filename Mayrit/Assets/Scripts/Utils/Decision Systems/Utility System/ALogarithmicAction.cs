using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for actions that have a logarithmic decision factor (float).
/// </summary>
public abstract class ALogarithmicAction : AAction<float>
{
    #region PROPERTIES
    readonly bool _inverted;
    #endregion

    #region CONSTRUCTOR
    protected ALogarithmicAction(string name, UtilitySystem utilitySystem, bool inverted = false)
    : base(name, utilitySystem)
    {
        _inverted = inverted;
    }
    #endregion

    #region INHERITED METHODS
    protected override float CalculateUtility()
    {
        _utility = (float)Math.Log(DecisionFactor + 1); // Logarithmic function

        if (_inverted)
            _utility = 1f - _utility; // Inverted logarithmic function

        //Debug.Log(name + " utility: " + utility);
        return _utility;
    }
    #endregion
}