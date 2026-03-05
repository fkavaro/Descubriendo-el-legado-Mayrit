using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class Orbital_CameraState : ACameraState
{
    #region PROPERTIES HELPERS
    public OrbitalStateSetting Setting
    {
        get => _setting;
        set => _setting = value;
    }
    #endregion

    #region PROPERTIES
    readonly OrbitalCameraController _controller;
    OrbitalStateSetting _setting;
    #endregion

    #region CONSTRUCTOR
    public Orbital_CameraState(OrbitalCameraDataSO orbitalCameraData, CinemachineCamera camera)
    : base("Orbital camera", camera, orbitalCameraData.SimulationSpeed)
    {
        _controller = new(orbitalCameraData, camera);
    }
    #endregion

    #region INHERITED METHODS
    public override void StartState()
    {
        base.StartState();

        _controller.Start(_setting);
        _uiManager.ShowContextualPanel(_setting.DataToShow);
    }

    public override void LateUpdateState()
    {
        if (_gameManager.IsInPauseState || _uiManager.IsInLoadingScreenState)
            return;

        _controller.LateUpdate();
    }

    #endregion
}