using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ASelection_TutorialStepConditionSO", menuName = "Scriptable Objects/Tutorial Conditions/Selection")]
public class ContextualPanelShownConditionSO : ATutorialStepConditionSO
{
    [SerializeField] List<DataSO.DataType> _dataTypes = new();
    UISystem _uiSystem;

    public void SetUISystem(UISystem uiSystem)
    {
        _uiSystem = uiSystem;
    }

    public override void BeginListening()
    {
        _uiSystem.StateChangedEvent += OnUIStateChanged;
    }

    public override void EndListening()
    {
        _uiSystem.StateChangedEvent -= OnUIStateChanged;
    }

    void OnUIStateChanged()
    {
        if (!_uiSystem.IsInInformationDisplayState) return;

        DataSO data = _uiSystem.InformationDisplayState.DataToShow;

        if (_dataTypes.Contains(data.Type))
        {
            EndListening();
            MarkCompleted();
        }
    }
}
