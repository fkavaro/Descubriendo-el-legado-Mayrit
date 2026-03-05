using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSO", menuName = "Scriptable Objects/TutorialStepSO")]
public class TutorialStepSO : ScriptableObject
{
    [SerializeField] string _stepVisualName;
    [SerializeField] ATutorialStepConditionSO _completionCondition;
    [SerializeField] float _animationDuration = 0.3f;
    [SerializeField] float _animationPeakScale = 1.1f;
    [SerializeField] List<UIElementsToHide> _visualElementsToHide;

    public string VisualElementName => _stepVisualName;
    public ATutorialStepConditionSO CompletionCondition => _completionCondition;
    public float AnimationDuration => _animationDuration;
    public float AnimationPeakScale => _animationPeakScale;
    public List<UIElementsToHide> VisualElementsToHide => _visualElementsToHide;
}

public enum UIElementsToHide
{
    TutorialMilestoneButtons,
    TutorialMilestoneArea,
    TutorialPlayerFollower,
    TutorialCompass,
    TutorialSwitches
}
