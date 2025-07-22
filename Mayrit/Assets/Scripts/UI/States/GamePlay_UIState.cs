using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class GamePlay_UIState : AUIState
{
    #region PRIVATE PROPERTIES
    Label _tooltip;
    Vector2 _cursorCreenPos;
    #endregion

    #region INHERITED
    public GamePlay_UIState(FiniteStateMachine<UIManager> stateMachine)
    : base("GamePlayUI", stateMachine) { }

    public override void AwakeState()
    {

    }

    public override void StartState()
    {
        UIManager.Instance.UIDocument = UIManager.Instance.GetComponent<UIDocument>();
        _UI = UIManager.Instance.UIDocument;
        _tooltip = _UI.rootVisualElement.Q<Label>("Tooltip");

        HideTooltip();
    }

    public override void UpdateState()
    {
        if (_tooltip.style.display == DisplayStyle.None) return;

        _cursorCreenPos = Mouse.current.position.ReadValue();

        // UI Toolkit's Y axis is from top to bottom, while screen coordinates are from bottom to top
        float tooltipX = _cursorCreenPos.x + UIManager.Instance.tooltipOffsetX;
        float tooltipY = Screen.height - _cursorCreenPos.y - UIManager.Instance.tooltipOffsetY;

        _tooltip.style.left = tooltipX;
        _tooltip.style.top = tooltipY;
    }
    #endregion

    #region PUBLIC METHODS
    public void PlaceTooltip(GameObject gameObject)
    {
        if (_tooltip == null) return;

        // TODO: take selectableObject name component
        _tooltip.text = gameObject.name;

        _tooltip.style.display = DisplayStyle.Flex; // Show tooltip
    }

    public void HideTooltip()
    {
        if (_tooltip == null)
            Debug.LogWarning("Tooltip not found");
        else
            _tooltip.style.display = DisplayStyle.None; // Hide by default
    }
    #endregion
}
