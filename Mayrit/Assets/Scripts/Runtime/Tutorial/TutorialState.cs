using UnityEngine;
using UnityEngine.UIElements;

public class TutorialState : AUIState
{
    readonly TutorialStepSO _data;
    readonly AStateMachine<TutorialState> _fsm;
    readonly ATutorialStepConditionSO _completionCondition;

    VisualElement _tutorialScreen;

    public TutorialState(TutorialStepSO tutorialStepData, UIDocument uiDocument, AStateMachine<TutorialState> fsm)
    : base(tutorialStepData.VisualElementName, uiDocument)
    {
        _data = tutorialStepData;
        _completionCondition = _data.CompletionCondition;
        _completionCondition.SetUIDocument(_UIDocument);
        _fsm = fsm;
    }

    protected override void ConfigureUIElementsOnAwake()
    {
        _tutorialScreen = GetByName<VisualElement>("TutorialSteps", _UIDocument.rootVisualElement);

        if (_tutorialScreen.style.display != DisplayStyle.None)
            _tutorialScreen.style.display = DisplayStyle.None;
    }

    public override void StartState()
    {
        _completionCondition.Completed += OnCompletionConditionCompleted;
        _completionCondition.BeginListening();

        if (_tutorialScreen.style.display != DisplayStyle.Flex)
            _tutorialScreen.style.display = DisplayStyle.Flex;

        base.StartState();
    }

    private void OnCompletionConditionCompleted()
    {
        _completionCondition.Completed -= OnCompletionConditionCompleted;
        _completionCondition.EndListening();

        _fsm.SwitchToNextStateInSequence(out int nextStateIndex);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _completionCondition.Tick(Time.deltaTime);
    }
}
