using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ThirdPerson_GameState : AGameState
{
    public ThirdPerson_GameState(GameManager gameManager)
    : base(gameManager, "ThirdPerson") { }

    public override void StartState()
    {
        base.StartState();

        UISystem.SwitchToPlayerHUDState();
        CameraSystem.SwitchToThirdPersonCamera();
        PlayableCharacter.SwitchToControlledState();

        _gameManager.InputActions.Player.Enable();
    }

    public override void ExitState()
    {
        base.ExitState();

        _gameManager.InputActions.Player.Disable();
    }
}