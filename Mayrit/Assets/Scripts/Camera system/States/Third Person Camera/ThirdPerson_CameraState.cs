using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ThirdPerson_CameraState : ACameraState
{
    readonly Transform _camera;

    public ThirdPerson_CameraState(FiniteStateMachine<CameraManager> stateMachine,
        Transform camera)
    : base("Third person", stateMachine)
    {
        _camera = camera;
    }

    public override void StartState()
    {
        GameManager.Instance._inputActions.Player.Enable();
        _camera.gameObject.SetActive(true);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        GameManager.Instance._inputActions.Player.Disable();
        _camera.gameObject.SetActive(false);
    }
}
