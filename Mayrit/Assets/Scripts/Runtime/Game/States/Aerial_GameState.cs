using System;
using UnityEngine;
public class Aerial_GameState : AGameState
{
    public Aerial_GameState(GameManager gameManager)
    : base(gameManager, "Aerial") { }

    public override void StartState()
    {
        base.StartState();

        UISystem.SwitchToAerialHUDState();
        CameraSystem.SwitchToAerialCamera();
        PlayableCharacter.SwitchToNotControlledState();
    }
}