using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_GameState : AState<FiniteStateMachine>
{
    public MainMenu_GameState(FiniteStateMachine stateMachine)
    : base("Main menu", stateMachine) { }

    public override void StartState()
    {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
            SceneManager.LoadScene("MainMenuScene");
    }
}
