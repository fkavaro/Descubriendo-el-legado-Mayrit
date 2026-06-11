using System;
using UnityEngine;

public class Pause_GameState : AGameState
{
    public Pause_GameState(GameManager gameManager)
    : base(gameManager, "Pause") { }

    public override void StartState()
    {
        base.StartState();

        Time.timeScale = 0f;


    }

    public override void ExitState()
    {
        Time.timeScale = _gameManager.SimulationSpeed;
    }
}
