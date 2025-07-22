using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlay_GameState : AGameState
{
    Label _tooltip;

    public GamePlay_GameState(FiniteStateMachine<GameManager> stateMachine) : base("Gameplay", stateMachine)
    {

    }

    public override void AwakeState()
    {
        // Get active UI
        _UI = GameObject.FindFirstObjectByType<UIDocument>();
        Debug.Log(_UI.name);
        GameManager.Instance.currentUI = _UI;

        _tooltip = _UI.rootVisualElement.Q<Label>("Tooltip");

        if (_tooltip == null)
        {
            Debug.LogWarning("Tooltip not found");
        }
        else
        {
            _tooltip.style.display = DisplayStyle.None; // Hide by default
        }
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }

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
        if (_tooltip == null) return;

        _tooltip.style.display = DisplayStyle.None; // Hide tooltip
    }
}
