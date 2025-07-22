using UnityEngine;
using UnityEngine.UIElements;


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
    Label tooltip;
    #endregion

    #region INHERITED
    protected override void OnAwake()
    {
        UIDocument = GetComponent<UIDocument>();
        tooltip = UIDocument.rootVisualElement.Q<Label>("Tooltip");

        if (tooltip == null)
        {
            Debug.LogWarning("Tooltip not found");
        }
        else
        {
            tooltip.style.display = DisplayStyle.None; // Hide by default
        }
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

        fsm.SetInitialState(gamePlayState);

        return fsm;
    }
    #endregion

    #region PUBLIC METHODS

    #endregion
}
