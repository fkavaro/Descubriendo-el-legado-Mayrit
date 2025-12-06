using System;
using UnityEngine;

public class Pause_GameState : AGameState
{
    public event Action<bool> GamePausedEvent;

    readonly TimeManager _timeManager;

    public Pause_GameState()
    : base("Pause")
    {
        // Get dependencies from Service Locator
        _timeManager = ServiceLocator.Instance.Get<TimeManager>();
    }

    public override void StartState()
    {
        Time.timeScale = 0f;

        GamePausedEvent?.Invoke(true);
    }

    public override void ExitState()
    {
        Time.timeScale = _timeManager.SimulationSpeed;

        GamePausedEvent?.Invoke(false);
    }
}
