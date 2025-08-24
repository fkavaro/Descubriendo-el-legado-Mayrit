using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FreeRoam_PlayableCharacterState : APlayableCharacterState
{
    public FreeRoam_PlayableCharacterState(FiniteStateMachine stateMachine, PlayableCharacter playableCharacter)
    : base("Free roam", stateMachine, playableCharacter)
    {
    }

    public override void StartState()
    {
        _playerController.Start();
    }

    public override void UpdateState()
    {
        _playerController.Update();
    }
}