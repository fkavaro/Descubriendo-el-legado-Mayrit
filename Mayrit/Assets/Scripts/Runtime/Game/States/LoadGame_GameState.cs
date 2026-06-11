using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame_GameState : AGameState
{
    public LoadGame_GameState(GameManager gameManager)
    : base(gameManager, "LoadGame") { }

    public override void StartState()
    {
        base.StartState();

        // Load Game Scene, if not already loaded
        if (!SceneManager.GetSceneByName(SceneDatabase.SceneName.GameplayScene.ToString()).isLoaded)
            _scenesController.NewTransitionPlan()
                .Load(SceneDatabase.SceneType.Session, SceneDatabase.SceneName.GameplayScene)
                .Load(SceneDatabase.SceneType.Milestone, _gameManager.ProgressManager.StoredMilestoneScene, setActive: true)
                .WithOverlay()
                .ClearAssets()
                .Perform();
    }
}
