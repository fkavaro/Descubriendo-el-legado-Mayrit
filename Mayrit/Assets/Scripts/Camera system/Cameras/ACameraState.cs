using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public abstract class ACameraState : AState
{
    #region PROPERTY HELPERS
    public CinemachineCamera Camera
    {
        get => _camera;
        set => _camera = value;
    }
    #endregion

    #region PROPERTIES
    protected CinemachineCamera _camera;
    protected readonly float _simulationSpeed;
    #endregion

    #region CONSTRUCTOR
    protected ACameraState(string name,
        CinemachineCamera camera,
        float simulationSpeed)
    : base(name)
    {
        _camera = camera;
        _simulationSpeed = simulationSpeed;
    }
    #endregion

    #region INHERITED METHODS
    public override void StartState()
    {
        OnStateStarted();

        if (_camera == null)
        {
            Debug.LogWarning($"Camera state '{_stateName}' started without a valid Cinemachine camera assigned.");
            return;
        }

        _camera.gameObject.SetActive(true);
        TimeManager.Instance.SetSimulationSpeed(_simulationSpeed);
    }

    public override void ExitState()
    {
        OnStateExited();

        _camera.gameObject.SetActive(false);
    }
    #endregion

    #region VIRTUAL METHODS
    public abstract void OnStateStarted();
    public abstract void OnStateExited();
    #endregion
}