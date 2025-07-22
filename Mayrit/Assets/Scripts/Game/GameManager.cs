using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{

    #region PUBLIC PROPERTIES
    // State Machine
    public FiniteStateMachine<GameManager> fsm;
    public MainMenu_GameState mainMenuState;
    public GamePlay_GameState gamePlayState;
    public Pause_GameState pauseState;
    #endregion

    #region PRIVATE PROPERTIES

    #endregion

    #region INHERITED
    protected override void OnAwake()
    {

    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }
    protected override ADecisionSystem<GameManager> CreateDecisionSystem()
    {
        fsm = new(this);

        mainMenuState = new(fsm);
        gamePlayState = new(fsm);
        pauseState = new(fsm);

        fsm.SetInitialState(gamePlayState);

        return fsm;
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS

    #endregion
}
