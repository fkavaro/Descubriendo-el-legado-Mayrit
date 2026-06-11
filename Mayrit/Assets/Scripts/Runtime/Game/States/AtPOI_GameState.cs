using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class AtPOI_GameState : AGameState
{
    public PointOfInterest PointOfInterest;

    public AtPOI_GameState(GameManager gameManager)
    : base(gameManager, "AtPOI") { }

    public override void StartState()
    {

    }

    public override void ExitState()
    {

    }
}