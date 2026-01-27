using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for actions that have a binary decision factor (true/false).
/// </summary>
public abstract class ABinaryAction : AAction<bool>
{
    #region PROPERTIES
    readonly bool _inverted;
    readonly float _maxValue = 1f;
    #endregion

    #region CONSTRUCTORS
    protected ABinaryAction(string name, UtilitySystem utilitySystem, bool inverted = false)
    : base(name, utilitySystem)
    {
        _inverted = inverted;
    }

    protected ABinaryAction(string name, UtilitySystem utilitySystem, float maxValue, bool inverted = false)
    : base(name, utilitySystem)
    {
        _inverted = inverted;
        _maxValue = maxValue;
    }
    #endregion

    #region INHERITED METHODS
    protected override float CalculateUtility()
    {
        if (_inverted)
        {
            if (DecisionFactor)
                _utility = 0f;
            else
                _utility = _maxValue;
        }
        else
        {
            if (DecisionFactor)
                _utility = _maxValue;
            else
                _utility = 0f;
        }

        return _utility;
    }
    #endregion
}
