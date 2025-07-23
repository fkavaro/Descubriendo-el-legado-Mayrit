using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class ACameraState : AState<CameraManager, FiniteStateMachine<CameraManager>>
{
    protected ACameraState(string name, FiniteStateMachine<CameraManager> stateMachine)
    : base(name, stateMachine) { }
}