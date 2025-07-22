using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Pause_GameState : AState<GameManager, FiniteStateMachine<GameManager>>
{
    public Pause_GameState(FiniteStateMachine<GameManager> stateMachine) : base("Pause", stateMachine)
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
