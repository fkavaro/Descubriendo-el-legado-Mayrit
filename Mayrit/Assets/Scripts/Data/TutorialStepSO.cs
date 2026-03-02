using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSO", menuName = "Scriptable Objects/TutorialStepSO")]
public class TutorialStepSO : ScriptableObject
{
    [SerializeField] string _visualElementName;

    public string VisualElementName => _visualElementName;
}
