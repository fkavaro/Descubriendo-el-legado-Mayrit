using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AHUDState : AUIState
{
    #region PROPERTIES
    public event Action OnPlayCharacterEvent;
    public event Action ContextualPanelHiddenEvent;

    protected ContextualPanel _contextualPanel;
    protected bool _wasContextualPanelShown;

    VisualElement _contextualPanelRoot;
    #endregion

    #region CONSTRUCTOR
    public AHUDState(string name, UIDocument uiDocument)
    : base(name, uiDocument) { }
    #endregion

    #region INHERITED METHODS
    public override void AwakeState()
    {
        if (_contextualPanelRoot == null)
            InitializeContextualPanelOnAwake();

        if (_contextualPanel == null)
        {
            Debug.LogError($"{_stateName} HUD State: Contextual Panel is null!");
            return;
        }

        base.AwakeState();

        // Show contextual panel root if it was shown before
        if (_wasContextualPanelShown)
            _contextualPanelRoot.style.display = DisplayStyle.Flex;

        // Subscribe to events
        _contextualPanel.PlayCharacterClickedEvent += () => OnPlayCharacterEvent?.Invoke();
        _contextualPanel.HiddenEvent += () => ContextualPanelHiddenEvent?.Invoke();
        _contextualPanel.ShownEvent += OnContextualPanelShowCallback;
        _contextualPanel.HiddenEvent += OnContextualPanelHiddenCallback;
    }

    public override void ExitState()
    {
        base.ExitState();

        // Hide contextual panel root
        _contextualPanelRoot.style.display = DisplayStyle.None;

        // Unsubscribe from events
        _contextualPanel.PlayCharacterClickedEvent -= () => OnPlayCharacterEvent?.Invoke();
        _contextualPanel.HiddenEvent -= () => ContextualPanelHiddenEvent?.Invoke();
        _contextualPanel.ShownEvent -= OnContextualPanelShowCallback;
        _contextualPanel.HiddenEvent -= OnContextualPanelHiddenCallback;
        _uiManager.ShowContextualPanelEvent -= ShowContextualPanel;
        _uiManager.HideContextualPanelEvent -= HideContextualPanel;
    }

    public override bool IsCursorOverUI()
    {
        // Check base UI elements and contextual panel
        return base.IsCursorOverUI() || IsCursorOver(_contextualPanelRoot);
    }

    protected override void GetServicesDependenciesOnStart()
    {
        base.GetServicesDependenciesOnStart();

        _uiManager.ShowContextualPanelEvent += ShowContextualPanel;
        _uiManager.HideContextualPanelEvent += HideContextualPanel;
    }
    #endregion

    #region PROTECTED METHODS
    protected void ShowContextualPanel(DataSO data, bool isCharacterData = false)
    {
        _wasContextualPanelShown = true;
        _contextualPanel.ShowInfo(data, isCharacterData);
    }

    protected void HideContextualPanel()
    {
        _wasContextualPanelShown = false;
        _contextualPanel.Hide();
    }

    protected void OnPauseClicked(ClickEvent evt)
    {
        _uiManager.SwitchToPauseState();
        _soundManager.PlayButtonClickSFX();
    }
    #endregion

    #region PRIVATE METHODS
    void InitializeContextualPanelOnAwake()
    {
        if (_UIDocument == null)
        {
            Debug.LogError($"{_stateName} HUD State: UIDocument is null!");
            return;
        }

        if (_UIDocument.rootVisualElement == null)
        {
            Debug.LogError($"{_stateName} HUD State: UIDocument rootVisualElement is null!");
            return;
        }

        VisualElement hudScreen = _UIDocument.rootVisualElement.Q<VisualElement>("HUD");

        if (hudScreen == null)
        {
            Debug.LogWarning($"{_stateName} HUD element not found in UIDocument");
            return;
        }

        _contextualPanelRoot = hudScreen.Q<VisualElement>("ContextualPanel");

        if (_contextualPanelRoot == null)
        {
            Debug.LogWarning($"{_stateName} _contextualPanel not found");
            return;
        }

        _contextualPanel = new(_contextualPanelRoot);
    }
    #endregion

    #region CALLBACK METHODS
    void OnContextualPanelShowCallback()
    {
        _wasContextualPanelShown = true;
        OnContextualPanelShown();
    }
    void OnContextualPanelHiddenCallback()
    {
        _wasContextualPanelShown = false;
        OnContextualPanelHidden();
    }
    #endregion

    #region VIRTUAL METHODS
    protected virtual void OnContextualPanelShown() { }
    protected virtual void OnContextualPanelHidden() { }
    #endregion
}
