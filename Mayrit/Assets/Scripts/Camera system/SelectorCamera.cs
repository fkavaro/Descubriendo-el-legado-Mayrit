using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SelectorCamera : MonoBehaviour
{
    #region PUBLIC PROPERTIES
    [Header("Object selection")]
    [HideInInspector]
    public UIManager _UIDocument;
    [Tooltip("Layer mask to define which objects are selectable.")]
    public LayerMask _selectableLayer;
    #endregion

    #region PRIVATE PROPERTIES
    GameInputActions _inputActions;
    GameObject _currentSelected = null,
        _currentHover = null;
    Vector2 _mousePosition;
    #endregion

    #region MONOBEHAVIOUR
    // When script is enabled
    void OnEnable()
    {
        _inputActions.Camera.Enable();
    }

    // When script is disabled or destroyed
    void OnDisable()
    {
        _inputActions.Camera.Disable();
    }

    void Awake()
    {
        _inputActions = new();
        _inputActions.Camera.Enable();
        _inputActions.Camera.Select.performed += OnSelectObject;
    }

    void Start()
    {
        _UIDocument = UIManager.Instance;
    }

    void Update()
    {
        // Get the current mouse position
        _mousePosition = Mouse.current.position.ReadValue();

        UpdateTooltip();
    }
    #endregion

    #region PUBLIC METHODS
    /// <returns>The currently selected GameObject, or null if none is selected.</returns>
    public GameObject GetCurrentSelection()
    {
        return _currentSelected;
    }
    /// <returns>The currently hovered GameObject, or null if none is hovered.</returns>
    public GameObject GetCurrentHover()
    {
        return _currentHover;
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// This method is called when the 'SelectObject' input action is performed.
    /// It handles the raycast logic for object selection.
    /// </summary>
    /// <param name="context">The context of the input action callback.</param>
    void OnSelectObject(InputAction.CallbackContext context)
    {
        // Cursor over UI element
        if (UIManager.Instance.hudState.IsCursorOverUI(_mousePosition))
        {
            if (_currentSelected != null)
                ResetSelection();

            return;
        }

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 120f);

        // Perform the raycast. It checks for colliders on objects within the specified 'selectableLayer'.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _selectableLayer))
        {
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // If the ray hits a new object (different from the current selection)
            if (hit.collider.gameObject != _currentSelected)
            {
                // Deselect the previously selected object (if any)
                ResetSelection();

                // Set the newly hit object as the current selection
                _currentSelected = hit.collider.gameObject;
                //Debug.Log("Object selected: " + currentSelection.name);

                // Display the information panel of the selected object
                ApplySelection(_currentSelected);
            }
        }
        else
        {
            //Debug.LogWarning("Raycast did not hit any selectable object.");
            // If the ray hits nothing, deselect any previously selected object
            ResetSelection();
        }
    }

    /// <summary>
    /// Display the information panel of the selected object.
    /// </summary>
    void ApplySelection(GameObject selectedHover)
    {
        selectedHover.transform.localScale *= 2;
    }

    /// <summary>
    /// Resets the current selection.
    /// </summary>
    void ResetSelection()
    {
        if (_currentSelected == null) return;

        _currentSelected.transform.localScale /= 2;
        _currentSelected = null;
    }

    private void UpdateTooltip()
    {
        // Cursor over UI element
        if (UIManager.Instance.hudState.IsCursorOverUI(_mousePosition))
        {
            if (_currentHover != null)
                ResetHover();

            return;
        }

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);

        // Perform the raycast. It checks for colliders on objects within the specified 'hoverableLayer'.
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _selectableLayer))
        {
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // If the ray hits an object and it's different from the currently hovered one
            if (hit.collider.gameObject != _currentHover && hit.collider.gameObject != _currentSelected)
            {
                // If there was a previous hover object, reset its state (un-highlight it)
                ResetHover();

                // Set the newly hit object as the current hover object
                _currentHover = hit.collider.gameObject;

                // Show a small tooltip with the object's name
                ApplyHover(_currentHover);
            }
        }
        else
        {
            // If the ray hits nothing, and there was a previously hovered object, reset its state
            if (_currentHover != null)
                ResetHover();
        }
    }

    /// <summary>
    /// Show a small tooltip with the object's name.
    /// </summary>
    void ApplyHover(GameObject hoverObject)
    {
        _UIDocument.hudState.PlaceTooltip(hoverObject);
    }

    /// <summary>
    /// Resets the current hover.
    /// </summary>
    void ResetHover()
    {
        if (_currentHover == null) return;

        _currentHover = null;
        _UIDocument.hudState.HideTooltip();
    }
    #endregion
}
