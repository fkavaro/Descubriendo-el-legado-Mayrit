using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson_CameraState : ACameraState
{
    ThirdPersonCameraController _cameraController;

    public ThirdPerson_CameraState(CinemachineCamera camera, float simulationSpeed)
    : base("Third person camera", camera, simulationSpeed) { }

    public override void StartState()
    {
        GameManager.Instance.InputActions.Player.Enable();
        GameManager.Instance.InputActions.Camera.ExitMode.Enable();
        GameManager.Instance.InputActions.Camera.ExitMode.performed += SwitchToSpectatorCamera;

        _camera.gameObject.SetActive(true);

        // Change HUD
        UIManager.Instance.SwitchToPlayerHUDState();

        // Adjust simulation speed
        TimeManager.Instance.SetSimulationSpeed(_simulationSpeed);

        _cameraController = new(_camera);
    }

    public override void LateUpdateState()
    {
        _cameraController.Update();
    }

    public override void ExitState()
    {
        GameManager.Instance.InputActions.Player.Disable();
        GameManager.Instance.InputActions.Camera.ExitMode.Disable();
        GameManager.Instance.InputActions.Camera.ExitMode.performed -= SwitchToSpectatorCamera;
        _camera.gameObject.SetActive(false);
    }

    void SwitchToSpectatorCamera(InputAction.CallbackContext context)
    {
        CameraManager.Instance.SwitchToSpectatorCamera();
    }
}
