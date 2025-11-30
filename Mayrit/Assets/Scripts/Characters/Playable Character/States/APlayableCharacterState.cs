using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayableCharacterState : AState
{
    protected readonly PlayableCharacter _playableCharacter;

    protected APlayableCharacterState(string name, PlayableCharacter playableCharacter)
    : base(name)
    {
        _playableCharacter = playableCharacter;
    }
}
