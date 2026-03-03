using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "InputActionConditionSO", menuName = "Scriptable Objects/Tutorial Conditions/Input Action")]
public class InputActionConditionSO : ATutorialStepConditionSO
{
    [SerializeField] InputActionReference _action;
    bool _enabledByThisCondition;

    public override void BeginListening()
    {
        if (_action == null || _action.action == null)
        {
            Debug.LogWarning("InputActionConditionSO has no action assigned.");
            return;
        }

        if (!_action.action.enabled)
        {
            _action.action.Enable();
            _enabledByThisCondition = true;
        }

        _action.action.performed += OnPerformed;
    }

    public override void EndListening()
    {
        if (_action?.action != null)
        {
            _action.action.performed -= OnPerformed;
            if (_enabledByThisCondition) _action.action.Disable();
        }

        _enabledByThisCondition = false;
    }

    void OnPerformed(InputAction.CallbackContext ctx)
    {
        EndListening();
        MarkCompleted();
    }
}