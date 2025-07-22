using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlay_UIState : AUIState
{
    #region PRIVATE PROPERTIES
    Label _tooltip;
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

    }
    #endregion

    #region PUBLIC METHODS
    public void PlaceTooltip(GameObject gameObject)
    {
        if (_tooltip == null) return;

        _tooltip.text = gameObject.name;
        Vector3 screen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        _tooltip.style.left = screen.x - (_tooltip.layout.width / 2);
        _tooltip.style.top = Screen.height - screen.y - 100;

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
