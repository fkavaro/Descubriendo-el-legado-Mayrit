using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]

/// <summary>
/// Manages the user interface states and data. Singleton.
/// </summary>
public class UIManager : ASingletonBehaviourControllable<UIManager>
{
    #region EDITOR PROPERTIES
    [Header("User Interface Document")]
    public UIDocument _UIDocument;

    [Header("Tooltip Settings")]
    public Vector2 _tooltipOffset = new(-30, -30);
    #endregion

    #region PROPERTIES
    public StackFiniteStateMachine _fsm;
    public MainMenu_UIState _mainMenuState;
    public SpectatorHUD_UIState _spectatorHUDState;
    public PlayerHUD_UIState _playerHUDState;
    public PauseMenu_UIState _pauseState;
    public HeritageMenu_UIState _heritageState;
    #endregion

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        // Singleton
        base.Awake();

        _UIDocument = GetComponent<UIDocument>();

        _fsm = new(this);

        _mainMenuState = new(_fsm);
        _spectatorHUDState = new(_fsm);
        _playerHUDState = new(_fsm);
        _pauseState = new(_fsm);
        _heritageState = new(_fsm);

        // Set initial state based on scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameScene")
            _fsm.SetInitialState(_spectatorHUDState);
        else
            _fsm.SetInitialState(_mainMenuState);
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS

    #endregion
}
