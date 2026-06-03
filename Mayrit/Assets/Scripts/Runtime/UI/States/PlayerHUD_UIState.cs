using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD_UIState : AHUDState
{
    #region  PROPERTIES
    Tour _currentTour;
    TourManager _tourManager;

    VisualElement _onTourEndVisual,
        _tourArea,
        _stopsArea;
    Label _nextStopLabel, _completedTourStopsLabel, _totalTourStopsLabel;
    #endregion

    #region CONSTRUCTOR
    public PlayerHUD_UIState(UIDocument uiDocument, float fadeInDuration, float fadeOutDuration)
    : base("PlayerHUD", uiDocument, fadeInDuration, fadeOutDuration) { }
    #endregion

    #region UI STATE INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        base.ConfigureUIElementsOnAwake();

        _tourArea = GetByName<VisualElement>("TourArea");
        _stopsArea = GetByName<VisualElement>("StopsArea", _tourArea);
        _nextStopLabel = GetByName<Label>("NextStop", _stopsArea);
        _completedTourStopsLabel = GetByName<Label>("CompletedTourStopsCount");
        _totalTourStopsLabel = GetByName<Label>("TotalTourStopsCount");
        _onTourEndVisual = GetByName<VisualElement>("OnTourEnd");
    }

    protected override void GetServicesDependenciesOnStart()
    {
        base.GetServicesDependenciesOnStart();

        _currentTour = ServiceLocator.Instance.Get<Tour>();
        _tourManager = ServiceLocator.Instance.Get<TourManager>();

        if (_currentTour == null)
            Debug.LogWarning("PlayerHUD_UIState: No Tour found in ServiceLocator on StartState");
        if (_tourManager == null)
            Debug.LogWarning("PlayerHUD_UIState: No TourManager found in ServiceLocator on StartState");
    }

    public override void StartState()
    {
        base.StartState();

        SubscribeToTourEvents();
        ShowTourEndVisual(_currentTour.IsCompleted);
        UpdateTourStopsUI();
        _compass.IsNextTourStopShown = true;
    }

    public override void ExitState()
    {
        base.ExitState();

        UnsubscribeFromTourEvents();

        // Unlock cursor and make it visible (has been lock in 3rd person camera state start)
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        _compass.IsNextTourStopShown = false;
    }
    #endregion

    #region HUD STATE INHERITED METHODS
    protected override void OnContextualPanelShown()
    {
        _tourArea.style.display = DisplayStyle.None;
        _onTourEndVisual.style.display = DisplayStyle.None;
    }

    protected override void OnContextualPanelHidden()
    {
        _tourArea.style.display = DisplayStyle.Flex;
        ShowTourEndVisual(_currentTour.IsCompleted);
    }
    #endregion

    #region PRIVATE METHODS
    void ShowTourEndVisual(bool show)
    {
        if (show)
        {
            _stopsArea.style.display = DisplayStyle.None;
            _onTourEndVisual.style.display = DisplayStyle.Flex;
        }
        else
        {
            _stopsArea.style.display = DisplayStyle.Flex;
            _onTourEndVisual.style.display = DisplayStyle.None;
        }
    }

    /*
        void PopulateTourStopsUI()
        {
            if (_currentTour == null || _stopsArea == null) return;

            _stopsArea.Clear();
            _tourStopLabels.Clear();

            TourStop[] tourStops = _currentTour.GetComponentsInChildren<TourStop>();
            bool hasSetNextStop = false;
            foreach (TourStop stop in tourStops)
            {
                if (stop.Data == null) continue;

                Label label = new(stop.Data.Header);
                label.AddToClassList("HUDText");
                if (stop == _currentTour.NextTourStop)
                {
                    label.AddToClassList("highlighted");
                    hasSetNextStop = true;
                }
                else if (!hasSetNextStop && !stop.IsVisited)
                {
                    label.AddToClassList("highlighted");
                    hasSetNextStop = true;
                }
                else if (stop.IsVisited)
                    label.AddToClassList("disabled");

                label.name = $"TourStop_{stop.GetInstanceID()}";
                _stopsArea.Add(label);
                _tourStopLabels[stop] = label;
            }
        }

        void ClearTourStopsUI()
        {
            if (_stopsArea == null) return;
            _stopsArea.Clear();
            _tourStopLabels.Clear();
        }
        */

    void UpdateTourStopsUI()
    {
        _nextStopLabel.text = _currentTour.NextStop != null ? $"{_currentTour.NextStop.Data.Header}" : "Tour completado!";
        _completedTourStopsLabel.text = $"{_currentTour.VisitedStopsCount}";
        _totalTourStopsLabel.text = $"{_currentTour.TotalStopsCount}";
    }

    void SubscribeToTourEvents()
    {
        if (_tourManager != null)
            _tourManager.TourStopVisitedEvent += OnTourStopVisited;
    }

    void UnsubscribeFromTourEvents()
    {
        if (_tourManager != null)
            _tourManager.TourStopVisitedEvent -= OnTourStopVisited;
    }

    void OnTourStopVisited(TourStop tourStop)
    {
        UpdateTourStopsUI();
    }
    #endregion
}