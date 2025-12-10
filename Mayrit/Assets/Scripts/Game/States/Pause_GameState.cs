using System;
using UnityEngine;

public class Pause_GameState : AGameState
{
    public event Action<bool> GamePausedEvent;
    TimeManager _timeManager;

    public Pause_GameState()
    : base("Pause") { }

    public override void StartState()
    {
        // Get dependencies from Service Locator
        _timeManager = ServiceLocator.Instance.Get<TimeManager>();

        Time.timeScale = 0f;

        GamePausedEvent?.Invoke(true);
    }

    public override void ExitState()
    {
        Time.timeScale = _timeManager.SimulationSpeed;

        GamePausedEvent?.Invoke(false);
    }
}
