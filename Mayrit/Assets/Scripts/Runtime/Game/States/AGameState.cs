using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class AGameState : AState
{
    protected ScenesController _scenesController;
    protected GameManager _gameManager;

    protected AGameState(GameManager gameManager, string name)
    : base(name)
    {
        _gameManager = gameManager;
    }

    public override void AwakeState()
    {
        _scenesController = ServiceLocator.Instance.Get<ScenesController>();
    }
}
