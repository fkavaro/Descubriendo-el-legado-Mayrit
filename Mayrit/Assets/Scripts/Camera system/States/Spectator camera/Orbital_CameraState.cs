using UnityEngine;
using Unity.Cinemachine;

public class Orbital_CameraState : ACameraState
{
    public InformationSO _information;
    public readonly CinemachineOrbitalFollow _orbitalFollow;

    float _orbitSpeed,
        _zoomValue;

    public Orbital_CameraState(FiniteStateMachine<CameraManager> stateMachine,
        CinemachineCamera camera)
        : base("Orbitational camera", stateMachine, camera)
    {
        _orbitalFollow = camera.GetComponent<CinemachineOrbitalFollow>();
    }

    public override void StartState()
    {
        _camera.gameObject.SetActive(true);

        // Is character information
        if (_information.InformationType == InformationSO.Type.Character)
        {
            _orbitSpeed = CameraManager.Instance._orbitalCharacterOrbitSpeed;
            _zoomValue = CameraManager.Instance._orbitalCharacterZoom;
        }
        // Other
        else
        {
            _orbitSpeed = CameraManager.Instance._orbitalBuildingOrbitSpeed;
            _zoomValue = CameraManager.Instance._orbitalBuildingZoom;
        }

        CameraManager.Instance.ZoomToCoroutine(_orbitalFollow, _zoomValue);
        UIManager.Instance._spectatorHUDState.ShowContextualPanel(_information);
    }

    public override void UpdateState()
    {
        // Orbit around target
        _orbitalFollow.HorizontalAxis.Value += _orbitSpeed * Time.deltaTime;
    }

    public override void ExitState()
    {
        _camera.gameObject.SetActive(false);
    }
}