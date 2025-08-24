using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class APlayableCharacterState : AState<FiniteStateMachine>
{
    protected readonly PlayableCharacter _playableCharacter;

    protected APlayableCharacterState(string name, FiniteStateMachine stateMachine, PlayableCharacter playableCharacter)
    : base(name, stateMachine)
    {
        _playableCharacter = playableCharacter;
    }
}
