using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD_UIState : AHUDState
{
    #region  PROPERTIES
    Button _pauseButton;
    VisualElement _tourArea;
    Label _tourName,
        _tourDescription;

    // Dependency Injection
    TourManager _tourManager;
    #endregion

    #region CONSTRUCTOR
    public PlayerHUD_UIState(UIDocument uiDocument)
    : base("PlayerHUD", uiDocument)
    {

    }
    #endregion

    #region UI STATE INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _pauseButton = _screen.Q<Button>("PauseButton");
        _tourArea = _screen.Q<VisualElement>("TourArea");
        _tourName = _tourArea.Q<Label>("Name");
        _tourDescription = _tourArea.Q<Label>("Description");

        if (_pauseButton == null)
            Debug.LogWarning("_pauseButton not found");
        if (_tourArea == null)
            Debug.LogWarning("TourArea not found");
        if (_tourName == null)
            Debug.LogWarning("_tourName not found");
        if (_tourDescription == null)
            Debug.LogWarning("_tourDescription not found");

        // Get dependency from Service Locator

    }

    protected override void RegisterCallbacksOnAwake()
    {
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseClicked);
    }

    protected override void GetServicesDependenciesOnStart()
    {
        base.GetServicesDependenciesOnStart();

        _tourManager = ServiceLocator.Instance.Get<TourManager>();
    }

    protected override void OnStartState()
    {
        // Overwrite HUD info with current tour info
        Tour currentTour = _tourManager.CurrentTour;

        if (currentTour != null)
        {
            _tourName.text = currentTour.Data.Header;
            _tourDescription.text = currentTour.Data.SubHeader;
        }

        if (!_wasContextualPanelShown)
            _tourArea.style.display = DisplayStyle.Flex;
    }
    #endregion

    #region HUD STATE INHERITED METHODS
    protected override void OnContextualPanelShown()
    {
        _tourArea.style.display = DisplayStyle.None;
    }

    protected override void OnContextualPanelHidden()
    {
        _tourArea.style.display = DisplayStyle.Flex;
    }
    #endregion
}