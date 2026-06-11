using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AtCollectible_GameState : AGameState
{
    public Collectible Collectible;

    public AtCollectible_GameState(GameManager gameManager)
    : base(gameManager, "AtCollectible") { }

    public override void StartState()
    {

    }

    public override void ExitState()
    {

    }
}