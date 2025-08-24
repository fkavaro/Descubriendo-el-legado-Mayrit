using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class AGameState : AState<FiniteStateMachine>
{
    protected AGameState(string name, FiniteStateMachine stateMachine)
    : base(name, stateMachine)
    {
    }
}
