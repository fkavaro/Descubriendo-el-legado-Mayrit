using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class AGameState : AState
{
    protected ScenesController _scenesController;

    protected AGameState(string name)
    : base(name) { }

    public override void AwakeState()
    {
        _scenesController = ServiceLocator.Instance.Get<ScenesController>();
    }
}
