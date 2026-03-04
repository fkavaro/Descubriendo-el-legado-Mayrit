using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ASelection_TutorialStepConditionSO", menuName = "Scriptable Objects/Tutorial Conditions/Selection")]
public class ContextualPanelShownConditionSO : ATutorialStepConditionSO
{
    [SerializeField] List<DataSO.DataType> _dataTypes = new();
    UIManager _uiManager;

    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public override void BeginListening()
    {
        _uiManager.ContextualPanelShownEvent += OnContextualPanelShown;
    }

    public override void EndListening()
    {
        _uiManager.ContextualPanelShownEvent -= OnContextualPanelShown;
    }

    void OnContextualPanelShown(DataSO data)
    {
        if (_dataTypes.Contains(data.Type))
        {
            EndListening();
            MarkCompleted();
        }
    }
}
