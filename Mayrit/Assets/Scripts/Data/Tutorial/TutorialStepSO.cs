using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSO", menuName = "Scriptable Objects/TutorialStepSO")]
public class TutorialStepSO : ScriptableObject
{
    [SerializeField] string _visualElementName;
    [SerializeField] ATutorialStepConditionSO _completionCondition;

    public string VisualElementName => _visualElementName;
    public ATutorialStepConditionSO CompletionCondition => _completionCondition;
}
