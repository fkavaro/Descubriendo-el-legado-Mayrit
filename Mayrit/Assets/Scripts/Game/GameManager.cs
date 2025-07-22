using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{

    #region PUBLIC PROPERTIES
    public UIDocument currentUI;
    public MainMenu_GameState mainMenuState;
    public GamePlay_GameState gamePlayState;
    public Pause_GameState pauseState;
    #endregion

    #region PRIVATE PROPERTIES
    FiniteStateMachine<GameManager> gameFSM;
    #endregion

    #region MONOBEHAVIOUR
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
        gameFSM = new(this);

        mainMenuState = new(gameFSM);
        gamePlayState = new(gameFSM);
        pauseState = new(gameFSM);

        //gameFSM.SetInitialState(gamePlayState);

        return gameFSM;
    }
    #endregion

    #region PUBLIC METHODS

    #endregion

    #region PRIVATE METHODS

    #endregion
}
