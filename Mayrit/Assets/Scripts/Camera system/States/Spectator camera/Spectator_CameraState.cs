
using Unity.Cinemachine;

public class Spectator_CameraState : ACameraState
{
    readonly SpectatorCameraController _cameraController;
    readonly SelectorCamera _selectorCamera;

    public Spectator_CameraState(FiniteStateMachine<CameraManager> stateMachine,
        CinemachineCamera camera)
    : base("Spectator camera", stateMachine, camera)
    {
        _cameraController = new(camera, CameraManager.Instance._moveSpeedZoomCurve);
        _selectorCamera = new(CameraManager.Instance._selectableLayer);
    }

    public override void StartState()
    {
        _camera.gameObject.SetActive(true);

        // Change HUD
        UIManager.Instance._fsm.SwitchState(UIManager.Instance._spectatorHUDState);

        // Able to select
        GameManager.Instance._inputActions.Camera.Enable();
        GameManager.Instance._inputActions.Camera.Select.performed += _selectorCamera.OnSelectObject;

        _cameraController.Start();
        _selectorCamera.Start();
    }

    public override void UpdateState()
    {
        _cameraController.Update();
        _selectorCamera.Update();
    }

    public override void LateUpdateState()
    {
        _cameraController.LateUpdate();
    }

    public override void ExitState()
    {
        _camera.gameObject.SetActive(false);

        // Unable to select
        GameManager.Instance._inputActions.Camera.Disable();
        GameManager.Instance._inputActions.Camera.Select.performed -= _selectorCamera.OnSelectObject;
    }
}