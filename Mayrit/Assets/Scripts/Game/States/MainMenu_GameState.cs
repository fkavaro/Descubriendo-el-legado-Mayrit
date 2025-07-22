using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenu_GameState : AState<GameManager, FiniteStateMachine<GameManager>>
{
    public MainMenu_GameState(FiniteStateMachine<GameManager> stateMachine) : base("Main menu", stateMachine)
    {

    }

    public override void StartState()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

}
