using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayableCharacterState : AState
{
    protected readonly PlayableCharacter _playableCharacter;
    protected GameManager _gameManager;

    protected APlayableCharacterState(string name, PlayableCharacter playableCharacter)
    : base(name)
    {
        _playableCharacter = playableCharacter;
    }

    protected override void GetServicesDependenciesOnStart()
    {
        _gameManager = ServiceLocator.Instance.Get<GameManager>();

    }
}
