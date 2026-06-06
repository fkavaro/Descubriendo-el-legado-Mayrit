
using System;
using Unity.Cinemachine;

public class Aerial_CameraState : ACameraState
{
    #region PROPERTIES
    readonly AerialCameraController _cameraController;
    #endregion

    #region CONSTRUCTOR
    public Aerial_CameraState(AerialCameraDataSO aerialCameraData, CinemachineCamera camera)
    : base("Aerial camera", camera, aerialCameraData.SimulationSpeed)
    {
        _cameraController = new(aerialCameraData, camera);
    }
    #endregion

    #region INHERITED METHODS
    public override void StartState()
    {
        base.StartState();

        _gameManager.InputActions.Camera.Enable();
    }

    public override void UpdateState()
    {
        if (_gameManager.IsInPauseState || _uiManager.IsInLoadingScreenState)
            return;
    }

    public override void LateUpdateState()
    {
        if (_gameManager.IsInPauseState || _uiManager.IsInLoadingScreenState)
            return;

        _cameraController.LateUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();

        _gameManager.InputActions.Camera.Disable();
    }
    #endregion
}