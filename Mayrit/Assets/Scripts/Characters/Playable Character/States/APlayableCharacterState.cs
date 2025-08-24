using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class APlayableCharacterState : AState<FiniteStateMachine>
{
    protected readonly PlayableCharacter _playableCharacter;
    protected readonly PlayerController _playerController;

    protected APlayableCharacterState(string name, FiniteStateMachine stateMachine, PlayableCharacter playableCharacter, CharacterController playerCharacterController)
    : base(name, stateMachine)
    {
        _playableCharacter = playableCharacter;
        _playerController = new(playableCharacter, playerCharacterController);
    }
}
