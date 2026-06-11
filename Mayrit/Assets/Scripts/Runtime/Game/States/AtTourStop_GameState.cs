using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AtTourStop_GameState : AGameState
{
    public TourStop TourStop;

    public AtTourStop_GameState(GameManager gameManager)
    : base(gameManager, "AtTourStop") { }

    public override void StartState()
    {

    }

    public override void ExitState()
    {

    }
}