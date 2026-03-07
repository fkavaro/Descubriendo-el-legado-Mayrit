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

    // Dependency Injection
    protected UIManager _uiManager;
    protected GameManager _gameManager;
    protected CameraManager _cameraManager;
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
    protected override void GetServicesDependenciesOnStart()
    {
        _uiManager = ServiceLocator.Instance.Get<UIManager>();
        _gameManager = ServiceLocator.Instance.Get<GameManager>();
        _cameraManager = ServiceLocator.Instance.Get<CameraManager>();
    }

    public override void StartState()
    {
        base.StartState();

        if (_camera == null)
        {
            Debug.LogWarning($"{StateName}: Cannot start because the CinemachineCamera reference is null.");
            return;
        }

        _gameManager.SetSimulationSpeed(_simulationSpeed);

        _camera.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();

        _camera.gameObject.SetActive(false);
    }
    #endregion
}