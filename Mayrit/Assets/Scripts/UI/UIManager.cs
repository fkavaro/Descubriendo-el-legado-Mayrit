using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    #region PUBLIC PROPERTIES
    [Header("User Interface Properties")]
    public UIDocument UIDocument;

    // State Machine
    public FiniteStateMachine<UIManager> fsm;
    public MainMenu_UIState mainMenuState;
    public GamePlay_UIState gamePlayState;
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

    protected override ADecisionSystem<UIManager> CreateDecisionSystem()
    {
        fsm = new(this);

        mainMenuState = new(fsm);
        gamePlayState = new(fsm);

        // Set initial state based on scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameScene")
        {
            fsm.SetInitialState(gamePlayState);
        }
        else
        {
            fsm.SetInitialState(mainMenuState);
        }

        return fsm;
    }
    #endregion

    #region PUBLIC METHODS

    #endregion
}
