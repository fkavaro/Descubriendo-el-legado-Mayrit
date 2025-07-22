using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public abstract class AGameState : AState<GameManager, FiniteStateMachine<GameManager>>
{
    public UIDocument _UI;

    protected AGameState(string name, FiniteStateMachine<GameManager> stateMachine) : base(name, stateMachine)
    {
    }
}
