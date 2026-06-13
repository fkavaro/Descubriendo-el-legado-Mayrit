using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class AtPOI_GameState : AGameState
{
    public DataSO Data;
    public OrbitalCameraSettings OrbitalCameraSettings;

    public AtPOI_GameState(GameManager gameManager)
    : base(gameManager, "AtPOI") { }

    public override void StartState()
    {
        base.StartState();

        UISystem.SwitchToInformationDisplayState(Data);
        CameraSystem.SwitchToOrbitalCamera(OrbitalCameraSettings);
        PlayableCharacter.SwitchToNotControlledState();
    }
}