using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public abstract class ACameraState : AState<CameraManager, FiniteStateMachine<CameraManager>>
{
    protected readonly CinemachineCamera _camera;

    protected ACameraState(string name,
        FiniteStateMachine<CameraManager> stateMachine,
        CinemachineCamera camera)
    : base(name, stateMachine)
    {
        _camera = camera;
    }
}