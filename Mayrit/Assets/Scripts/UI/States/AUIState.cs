using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public abstract class AUIState : AState
{
    #region PROPERTIES
    public UIDocument _UIDocument;
    public VisualElement _screen;

    // Dependency Injection
    protected readonly UIManager _uiManager;
    protected readonly GameManager _gameManager;
    #endregion

    #region CONSTRUCTOR
    protected AUIState(string name, UIDocument uiDocument)
    : base(name)
    {
        _UIDocument = uiDocument;

        // Get dependencies from Service Locator
        _uiManager = ServiceLocator.Instance.Get<UIManager>();
        _gameManager = ServiceLocator.Instance.Get<GameManager>();
    }
    #endregion

    #region INHERITED METHODS
    public override void StartState()
    {
        _screen = _UIDocument.rootVisualElement.Q<VisualElement>(_stateName);

        ConfigureUIElements();
        RegisterCallbacks();
        OnStartState();

        _screen.style.display = DisplayStyle.Flex; // Show
    }

    public override void ExitState()
    {
        _screen.style.display = DisplayStyle.None; // Hide
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Returns true if the cursor is over any UI element that is a descendant of _screen.
    /// </summary>
    /// <param name="cursorPos">Screen-space position of the cursor (Input.mousePosition).</param>
    public virtual bool IsCursorOverUI()
    {
        return IsCursorOver(_screen);
    }
    #endregion

    #region PRIVATE METHODS
    protected bool IsCursorOver(VisualElement uiElement)
    {
        // Get the current mouse position
        Vector2 cursorPos = Mouse.current.position.ReadValue();

        // Convert to UI Toolkit coordinates (Y is flipped)
        Vector2 panelPosition = new(cursorPos.x, Screen.height - cursorPos.y);

        // Pick the topmost VisualElement at the given position
        var panel = uiElement.panel;
        VisualElement pickedElement = panel?.Pick(panelPosition);

        // Check if the picked element is a descendant of element (and not element itself)
        if (pickedElement != null && pickedElement != uiElement && uiElement.Contains(pickedElement))
            return true;

        return false;
    }
    #endregion

    #region ABSTRACT METHODS
    protected abstract void ConfigureUIElements();
    protected abstract void RegisterCallbacks();
    protected virtual void OnStartState() { }
    #endregion
}
