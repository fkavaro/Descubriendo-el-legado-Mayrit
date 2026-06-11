using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InformationDisplay_UIState : AUIState
{
    #region PROPERTIES
    public event Action PlayTourClickedEvent;
    public event Action ResetTourClickedEvent;
    public event Action ClosedEvent;

    public DataSO DataToShow;

    readonly ContextualPanelComponent _contextualPanelComponent;

    Button _pauseButton;
    #endregion


    public InformationDisplay_UIState(UISystem uiSystem, UIDocument uiDocument, float fadeInDuration, float fadeOutDuration, ContextualPanelComponent contextualPanelComponent)
    : base(uiSystem, "InformationDisplay", uiDocument, fadeInDuration, fadeOutDuration)
    {
        _contextualPanelComponent = contextualPanelComponent;
    }

    protected override void ConfigureUIElementsOnAwake()
    {
        _pauseButton = GetButtonAndRegisterCallback("PauseButton", OnPauseClicked);

        _contextualPanelComponent.ContinueClickedEvent += OnStartTour;
        _contextualPanelComponent.ResetTourClickedEvent += OnResetTour;
        _contextualPanelComponent.ClosedEvent += OnCloseButton;
    }

    public override void StartState()
    {
        base.StartState();

        if (DataToShow == null)
        {
            Debug.LogError("InformationDisplay_UIState: DataToShow is null!");
            return;
        }

        _gameManager.InputActions.UI.Enable();
        _gameManager.InputActions.UI.Pause.performed += OnPauseKeyPressed;

        _contextualPanelComponent.ShowData(DataToShow);

    }

    public override void ExitState()
    {
        base.ExitState();

        _contextualPanelComponent.ExitState();

        _gameManager.InputActions.UI.Disable();
        _gameManager.InputActions.UI.Pause.performed -= OnPauseKeyPressed;
    }

    void OnPauseKeyPressed(InputAction.CallbackContext context)
    {
        OnPauseClicked(null);
    }

    void OnPauseClicked(ClickEvent evt)
    {
        _uiSystem.SwitchToPauseState();
        _soundManager.PlayButtonClickSFX();
    }

    void OnCloseButton()
    {
        ClosedEvent?.Invoke();
    }

    void OnStartTour()
    {
        _soundManager.PlayTourStartSFX();
        PlayTourClickedEvent?.Invoke();
    }

    void OnResetTour()
    {
        _soundManager.PlayTourStartSFX();
        ResetTourClickedEvent?.Invoke();
    }
}
