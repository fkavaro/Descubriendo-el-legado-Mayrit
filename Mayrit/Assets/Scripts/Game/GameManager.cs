using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game states and data. Singleton.
/// </summary>
public class GameManager : ASingletonBehaviourControllable<GameManager>
{
    #region EDITOR PROPERTIES
    [Header("Player")]
    public PlayableCharacter _currentPlayableCharacter;
    #endregion

    #region PROPERTIES
    public FiniteStateMachine _fsm;
    public MainMenu_GameState _mainMenuState;
    public GamePlay_GameState _gamePlayState;
    public Pause_GameState _pauseState;

    public GameInputActions _inputActions;
    #endregion

    public override ADecisionSystem CreateDecisionSystem()
    {
        // FINITE STATE MACHINE
        _fsm = new(this);

        _mainMenuState = new(_fsm);
        _gamePlayState = new(_fsm);
        _pauseState = new(_fsm);

        // Set initial state based on scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameScene")
            _fsm.SetInitialState(_gamePlayState);
        else
            _fsm.SetInitialState(_mainMenuState);

        return _fsm;
    }

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        // Singleton
        base.Awake();

        _inputActions = new();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        _inputActions?.Disable(); // Disables all action maps. To avoid errors
    }

    public PlayableCharacter GetCurrentPlayableCharacter()
    {
        // Find the player character
        _currentPlayableCharacter = FindFirstObjectByType<PlayableCharacter>();

        return _currentPlayableCharacter;
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS

    #endregion
}
