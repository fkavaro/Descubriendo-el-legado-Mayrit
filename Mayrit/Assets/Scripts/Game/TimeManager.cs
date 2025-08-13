using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class TimeManager : MonoBehaviour
{
    #region PUBLIC PROPERTIES
    [Header("Time Settings")]
    [Tooltip("Current time in hours since the start of the game")]
    [Range(0f, 24f)]
    public float _currentTime = 8f; // Default starting time at 8 AM

    [Tooltip("Time speed multiplier for the game")]
    public float _timeSpeed = 1f; // Speed at which time passes

    [Tooltip("Whether current time is between 6 and 18 hours or not")]
    public bool _isDayTime = true;

    public float _normalisedTime;

    [Header("Light Settings")]
    [Tooltip("Sun light source")]
    public Light _sunLight;
    public float _sunAngle;
    public float _sunPosition = 90f;
    public float _sunMaxIntensity;
    public AnimationCurve _sunIntensityCurve;
    public AnimationCurve _sunTemperatureCurve;
    #endregion

    #region PRIVATE PROPERTIES

    #endregion

    #region INHERITED METHODS
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        UpdateLighting();
        CheckShadows();
    }

    // Called when the script is loaded or a value is changed in the inspector
    void OnValidate()
    {
        UpdateLighting();
        CheckShadows();
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS
    void UpdateTimeOfDay()
    {
        // Update the current time based on the time speed
        _currentTime += Time.deltaTime * _timeSpeed;

        // Ensure current time wraps around after 24 hours
        if (_currentTime >= 24f)
            _currentTime = 0f;
    }

    void UpdateLighting()
    {
        _sunAngle = _currentTime / 24f * 360f; // Calculate sun angle based on current time

        // Rotate sun light source
        _sunLight.transform.rotation = Quaternion.Euler(_sunAngle - 90f, _sunPosition, 0f); // -90 so that its down at midnight

        // Normalise current time to a value between 0 and 1
        _normalisedTime = _currentTime / 24f;

        // Set the intensity of the sun light based on the evaluated curve
        _sunLight.intensity = _sunMaxIntensity * _sunIntensityCurve.Evaluate(_normalisedTime);
        // Set the color temperature of the sun light based on the evaluated curve
        _sunLight.colorTemperature = 10000f * _sunTemperatureCurve.Evaluate(_normalisedTime); // In kelvin units
    }

    void CheckShadows()
    {
        // During day time
        if (_currentTime >= 6f && _currentTime < 18f) // Between 6 AM and 6 PM
        {
            _isDayTime = true;

            // Check if shadows are disabled and enable them
            if (_sunLight.shadows == LightShadows.None)
                _sunLight.shadows = LightShadows.Hard; // Enable shadows
        }
        else // During night time
        {
            _isDayTime = false;

            // Check if shadows are enabled and disable them
            if (_sunLight.shadows != LightShadows.None)
                _sunLight.shadows = LightShadows.None; // Disable shadows
        }
    }
    #endregion
}
