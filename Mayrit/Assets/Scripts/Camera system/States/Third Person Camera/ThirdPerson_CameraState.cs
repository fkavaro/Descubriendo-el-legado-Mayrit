using Unity.Cinemachine;

public class ThirdPerson_CameraState : ACameraState
{
    ThirdPersonCameraController _cameraController;

    public ThirdPerson_CameraState(FiniteStateMachine<CameraManager> stateMachine,
        CinemachineCamera camera)
    : base("Third person camera", stateMachine, camera) { }

    public override void StartState()
    {
        GameManager.Instance._inputActions.Player.Enable();
        _camera.gameObject.SetActive(true);

        // Change HUD
        UIManager.Instance._fsm.SwitchState(UIManager.Instance._playerHUDState);

        _cameraController = new(_camera);
    }

    public override void LateUpdateState()
    {
        _cameraController.LateUpdate();
    }

    public override void ExitState()
    {
        GameManager.Instance._inputActions.Player.Disable();
        _camera.gameObject.SetActive(false);
    }
}
