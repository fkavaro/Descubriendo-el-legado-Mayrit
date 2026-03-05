using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassUI : AUIState
{
    #region PROPERTIES
    readonly VisualElement _root;

    VisualElement _cardinalDirections, _nextPOIVisual, _nextPoiDirection;

    bool _isNextPOIShown;
    Camera _mainCamera;
    PointOfInterest _nextPOI;

    // Dependency Injection
    TourManager _tourManager;
    #endregion

    #region CONSTRUCTOR
    public CompassUI(UIDocument uIDocument, VisualElement root) : base("Compass", uIDocument)
    {
        if (root == null)
        {
            Debug.LogError("CompassUI: Root VisualElement is null");
            return;
        }

        _root = root;
    }

    protected override void ConfigureUIElementsOnAwake()
    {
        _cardinalDirections = GetByName<VisualElement>("CardinalDirections", _root);
        _nextPOIVisual = GetByName<VisualElement>("NextPOI", _root);
        _nextPoiDirection = GetByName<VisualElement>("NextPOIDirection", _root);
    }
    #endregion

    #region PUBLIC METHODS
    public override void AwakeState()
    {
        base.AwakeState();

        IsShown(false);
        IsNextPOIShown(false);
    }

    public override void StartState()
    {
        _mainCamera = Camera.main; // TODO get from Camera Manager
        FixCardinalDirections();
    }

    public override void UpdateState()
    {
        _mainCamera = Camera.main; // TODO get from Camera Manager
        FixCardinalDirections();

        if (_isNextPOIShown)
            FixPOIDirection();
    }

    public void IsShown(bool isShown)
    {
        _root.style.display = isShown ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void IsNextPOIShown(bool isShown)
    {
        _isNextPOIShown = isShown;

        if (isShown)
        {
            _nextPOIVisual.style.display = DisplayStyle.Flex;

            if (_tourManager == null)
                _tourManager = ServiceLocator.Instance.Get<TourManager>();

            if (_tourManager == null)
            {
                Debug.LogWarning("CompassUI: TourManager not found in ServiceLocator");
                _isNextPOIShown = false;
            }
        }
        else
        {
            _nextPOIVisual.style.display = DisplayStyle.None;
            _nextPoiDirection.style.display = DisplayStyle.None;

            _tourManager = null;
        }
    }
    #endregion

    #region PRIVATE METHODS
    void FixCardinalDirections()
    {
        // Rotate the cardinal directions in the opposite direction of the camera's Y rotation
        float cameraYRotation = _mainCamera.transform.eulerAngles.y;
        _cardinalDirections.style.rotate = new Rotate(-cameraYRotation);
    }

    void FixPOIDirection()
    {
        if (_tourManager.CurrentTour == null || _tourManager.CurrentTour.NextPOI == null)
        {
            _nextPOIVisual.style.display = DisplayStyle.None;
            _nextPoiDirection.style.display = DisplayStyle.None;
            return;
        }

        _nextPOI = _tourManager.CurrentTour.NextPOI;

        // Get the direction to the POI in world space
        Vector3 toPoi = _nextPOI.transform.position - _mainCamera.transform.position;
        Vector3 flatToPoi = Vector3.ProjectOnPlane(toPoi, Vector3.up).normalized;
        Vector3 flatForward = Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up).normalized;

        if (flatToPoi.sqrMagnitude > 0.0001f && flatForward.sqrMagnitude > 0.0001f)
        {
            float angle = Vector3.SignedAngle(flatForward, flatToPoi, Vector3.up);
            _nextPoiDirection.style.rotate = new Rotate(angle);
        }

        if (_nextPOIVisual.style.display == DisplayStyle.None)
            _nextPOIVisual.style.display = DisplayStyle.Flex;

        if (_nextPoiDirection.style.display == DisplayStyle.None)
            _nextPoiDirection.style.display = DisplayStyle.Flex;
    }
    #endregion
}
