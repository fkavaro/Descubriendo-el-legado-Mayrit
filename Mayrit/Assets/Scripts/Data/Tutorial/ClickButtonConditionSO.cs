using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ClickButtonConditionSO", menuName = "Scriptable Objects/Tutorial Conditions/Click Button")]
public class ClickButtonConditionSO : ATutorialStepConditionSO
{
    [HideInInspector] public UIDocument UIDocument;
    [SerializeField] string _buttonName;
    Button _button;

    public override void BeginListening()
    {
        _button = UIDocument.rootVisualElement.Q<Button>(_buttonName);
        if (_button == null)
        {
            Debug.LogWarning($"Button '{_buttonName}' not found for tutorial condition.");
            return;
        }

        _button.clicked += OnClicked;
    }

    public override void EndListening()
    {
        if (_button != null) _button.clicked -= OnClicked;
        _button = null;
    }

    void OnClicked()
    {
        EndListening();
        MarkCompleted();
    }
}