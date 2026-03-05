using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialState : AUIState
{
    readonly TutorialStepSO _data;
    readonly AStateMachine<TutorialState> _fsm;
    readonly ATutorialStepConditionSO _completionCondition;
    readonly List<VisualElement> _hiddenElements = new();

    VisualElement _hudScreen, _tutorialStepsParent;

    public TutorialState(TutorialStepSO tutorialStepData, UIManager uiManager, AStateMachine<TutorialState> fsm)
    : base(tutorialStepData.VisualElementName, uiManager.UIDocument)
    {
        _data = tutorialStepData;
        _completionCondition = _data.CompletionCondition;

        _completionCondition.SetUIDocument(_UIDocument);

        if (_completionCondition is ContextualPanelShownConditionSO selectionCondition)
            selectionCondition.SetUIManager(uiManager);

        _fsm = fsm;
    }

    protected override void ConfigureUIElementsOnAwake()
    {
        _hudScreen = GetByName<VisualElement>("HUD", _UIDocument.rootVisualElement);
        _tutorialStepsParent = GetByName<VisualElement>("TutorialSteps", _hudScreen);

        if (_tutorialStepsParent.style.display != DisplayStyle.None)
            _tutorialStepsParent.style.display = DisplayStyle.None;

        foreach (UIElementsToHide elementName in _data.VisualElementsToHide)
        {
            VisualElement elementToHide = GetByName<VisualElement>(elementName.ToString(), _hudScreen);

            if (elementToHide != null)
                _hiddenElements.Add(elementToHide);
        }
    }

    public override void StartState()
    {
        _completionCondition.Completed += OnCompletionConditionCompleted;
        _completionCondition.BeginListening();

        if (_tutorialStepsParent.style.display != DisplayStyle.Flex)
            _tutorialStepsParent.style.display = DisplayStyle.Flex;

        foreach (VisualElement element in _hiddenElements)
        {
            element.style.display = DisplayStyle.None;
            Debug.Log($"{_stateName}: Hiding element {element.name}");
        }


        base.StartState();
    }

    private void OnCompletionConditionCompleted()
    {
        _completionCondition.Completed -= OnCompletionConditionCompleted;
        _completionCondition.EndListening();

        foreach (VisualElement element in _hiddenElements)
            element.style.display = DisplayStyle.Flex;

        _hiddenElements.Clear();

        _fsm.SwitchToNextStateInSequence(out int newStateIndex);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _completionCondition.Tick(Time.deltaTime);
    }
}
