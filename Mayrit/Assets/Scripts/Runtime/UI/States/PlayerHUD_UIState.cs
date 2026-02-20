using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD_UIState : AHUDState
{
    #region  PROPERTIES
    public bool _showTourEnd;
    Tour _currentTour;

    Button _pauseButton;
    VisualElement _tourArea,
        _onTourEndVisual;
    Label _tourName,
        _tourDescription;
    #endregion

    #region CONSTRUCTOR
    public PlayerHUD_UIState(UIDocument uiDocument)
    : base("PlayerHUD", uiDocument) { }
    #endregion

    #region UI STATE INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        base.ConfigureUIElementsOnAwake();

        _pauseButton = _screen.Q<Button>("PauseButton");
        _tourArea = _screen.Q<VisualElement>("TourArea");
        _tourName = _tourArea.Q<Label>("Name");
        _tourDescription = _tourArea.Q<Label>("Description");
        _onTourEndVisual = _screen.Q<VisualElement>("OnTourEnd");

        if (_pauseButton == null)
            Debug.LogWarning("_pauseButton not found");
        if (_tourArea == null)
            Debug.LogWarning("TourArea not found");
        if (_tourName == null)
            Debug.LogWarning("_tourName not found");
        if (_tourDescription == null)
            Debug.LogWarning("_tourDescription not found");
        if (_onTourEndVisual == null)
            Debug.LogWarning("_onTourEndVisual not found");
    }

    protected override void RegisterUICallbacksOnAwake()
    {
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseClicked);
    }

    protected override void GetServicesDependenciesOnStart()
    {
        base.GetServicesDependenciesOnStart();

        _currentTour = ServiceLocator.Instance.Get<Tour>();

        if (_currentTour == null)
            Debug.LogWarning("PlayerHUD_UIState: No Tour found in ServiceLocator on StartState");
    }

    public override void StartState()
    {
        base.StartState();

        ShowTourEndVisual(_showTourEnd);
    }

    public override void ExitState()
    {
        base.ExitState();

        // Unlock cursor and make it visible (has been lock in 3rd person camera state start)
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    #region HUD STATE INHERITED METHODS
    protected override void OnContextualPanelShown()
    {
        _tourArea.style.display = DisplayStyle.None;
        _onTourEndVisual.style.display = DisplayStyle.None;
    }

    protected override void OnContextualPanelHidden()
    {
        ShowTourEndVisual(_currentTour.IsCompleted);
    }
    #endregion

    #region PUBLIC METHODS
    void ShowTourEndVisual(bool show)
    {
        if (show)
        {
            _tourArea.style.display = DisplayStyle.None;
            _onTourEndVisual.style.display = DisplayStyle.Flex;
        }
        else
        {
            _onTourEndVisual.style.display = DisplayStyle.None;
            _tourArea.style.display = DisplayStyle.Flex;
            _tourName.text = _currentTour.Data.Header;
            _tourDescription.text = _currentTour.Data.SubHeader;
        }
    }
    #endregion
}