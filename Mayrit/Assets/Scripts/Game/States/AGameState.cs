using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class AGameState : AState<GameManager, FiniteStateMachine<GameManager>>
{
    protected AGameState(string name, FiniteStateMachine<GameManager> stateMachine)
    : base(name, stateMachine)
    {
    }
}
