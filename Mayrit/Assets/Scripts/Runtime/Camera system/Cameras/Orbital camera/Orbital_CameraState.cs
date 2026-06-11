using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class Orbital_CameraState : ACameraState
{
    #region PROPERTIES
    readonly OrbitalCameraController _controller;
    public OrbitalStateSetting Setting;
    #endregion

    #region CONSTRUCTOR
    public Orbital_CameraState(CameraSystem cameraManager, OrbitalCameraDataSO orbitalCameraData, CinemachineCamera camera)
    : base(cameraManager, "Orbital camera", camera, orbitalCameraData.SimulationSpeed)
    {
        _controller = new(orbitalCameraData, camera);
    }
    #endregion

    #region INHERITED METHODS
    public override void StartState()
    {
        base.StartState();

        _gameManager.InputActions.Camera.Enable();

        _controller.Start(Setting);
    }

    public override void LateUpdateState()
    {
        if (_gameManager.IsInPauseState)
            return;

        _controller.LateUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();

        _gameManager.InputActions.Camera.Disable();
    }
    #endregion
}