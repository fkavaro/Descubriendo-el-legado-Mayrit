
using System;
using Unity.Cinemachine;

public class Spectator_CameraState : ACameraState
{
    public event Action<SelectableObject> ObjectSelectedEvent;

    readonly SpectatorCameraController _cameraController;
    readonly SpectatorCameraSelector _cameraSelector;

    public Spectator_CameraState(CinemachineCamera camera, float simulationSpeed)
    : base("Spectator camera", camera, simulationSpeed)
    {
        _cameraController = new(camera, CameraManager.Instance._moveSpeedZoomCurve);
        _cameraSelector = new(CameraManager.Instance._selectableLayer);
    }

    public override void OnStateStarted()
    {
        GameManager.Instance.InputActions.Camera.Enable();
        UIManager.Instance.SwitchToSpectatorHUDState();
        _cameraSelector.ObjectSelectedEvent += OnObjectSelected;
    }

    public override void UpdateState()
    {
        _cameraController.Update();
        _cameraSelector.Update();
    }

    public override void LateUpdateState()
    {
        _cameraController.LateUpdate();
    }

    public override void OnStateExited()
    {
        GameManager.Instance.InputActions.Camera.Disable();
    }

    void OnObjectSelected(SelectableObject selectedObject)
    {
        ObjectSelectedEvent?.Invoke(selectedObject);
    }
}