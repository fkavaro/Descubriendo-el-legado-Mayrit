using UnityEngine;
using UnityEngine.UIElements;

public class TutorialState : AUIState
{
    private readonly TutorialStepSO _tutorialStepData;

    public TutorialState(TutorialStepSO tutorialStepData, UIDocument uiDocument)
    : base(tutorialStepData.VisualElementName, uiDocument)
    {
        _tutorialStepData = tutorialStepData;
    }

    protected override void ConfigureUIElementsOnAwake()
    {

    }

    protected override void RegisterUICallbacksOnAwake()
    {

    }
}
