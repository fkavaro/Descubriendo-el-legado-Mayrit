using UnityEngine;
using Unity.Cinemachine;

public class Orbital_CameraState : ACameraState
{
    public readonly CinemachineOrbitalFollow _orbitalFollow;

    public Orbital_CameraState(FiniteStateMachine<CameraManager> stateMachine, CinemachineCamera camera)
        : base("Orbitational camera", stateMachine, camera)
    {
        _orbitalFollow = camera.GetComponent<CinemachineOrbitalFollow>();
    }

    public override void StartState()
    {
        _camera.gameObject.SetActive(true);

        GameManager.Instance._inputActions.Camera.Enable();
        GameManager.Instance._inputActions.Camera.Move.Disable();
        GameManager.Instance._inputActions.Camera.Rotate.Disable();
    }

    public override void UpdateState()
    {
        // Orbit around target
        _orbitalFollow.HorizontalAxis.Value
        += CameraManager.Instance._orbitSpeed;

        // Change smoothly zoom to value set in CameraManager
        if (_orbitalFollow.RadialAxis.Value
            < CameraManager.Instance._orbitalCameraZoomValue
                - CameraManager.Instance._orbitalTransitionSpeed)
        {
            _orbitalFollow.RadialAxis.Value
            += CameraManager.Instance._orbitalTransitionSpeed;
        }
        else if (_orbitalFollow.RadialAxis.Value
            > CameraManager.Instance._orbitalCameraZoomValue
                + CameraManager.Instance._orbitalTransitionSpeed)
        {
            _orbitalFollow.RadialAxis.Value
            -= CameraManager.Instance._orbitalTransitionSpeed;
        }
    }

    public override void ExitState()
    {
        _camera.gameObject.SetActive(false);

        GameManager.Instance._inputActions.Camera.Disable();
    }
}