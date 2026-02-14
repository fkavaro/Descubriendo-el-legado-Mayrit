using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class AGameState : AState
{
    protected ScenesController _scenesController;
    protected GameManager _gameManager;
    protected ProgressManager _progressManager;

    protected AGameState(string name)
    : base(name) { }

    public override void AwakeState()
    {
        _scenesController = ServiceLocator.Instance.Get<ScenesController>();
    }

    protected override void GetServicesDependenciesOnStart()
    {
        _gameManager = ServiceLocator.Instance.Get<GameManager>();
        _progressManager = ServiceLocator.Instance.Get<ProgressManager>();
    }
}
