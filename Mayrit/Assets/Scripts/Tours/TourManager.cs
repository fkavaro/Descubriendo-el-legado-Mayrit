using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TourManager : Singleton<TourManager>
{
    #region PROPERTY HELPERS
    public Tour CurrentTour =>
    (_currentTourIndex >= 0 && _currentTourIndex < _tours.Count) ?
        _tours[_currentTourIndex] :
        null;
    #endregion

    #region EDITOR PROPERTIES
    public UnityEvent OnTourStarted;
    public UnityEvent OnAllToursCompleted;
    public UnityEvent OnTourChanged;

    [Tooltip("Player transform used to check POI visits")]
    public Transform _player;

    [Tooltip("Ordered tours as milestones order")]
    public List<Tour> _tours = new();
    #endregion

    #region INTERNAL PROPERTIES
    int _currentTourIndex = -1;
    #endregion

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        Reset();
        NextTour();
    }
    #endregion

    #region PUBLIC METHODS
    public void Reset()
    {
        _currentTourIndex = -1;
        ResetTours();
    }
    #endregion

    #region PRIVATE METHODS
    void NextTour()
    {
        // Handle last tour
        if (CurrentTour != null)
        {
            CurrentTour.OnTourCompleted.RemoveListener(OnTourCompleted);
            CurrentTour.Deactivate();
        }

        _currentTourIndex++;

        // All tours visited
        if (_currentTourIndex >= _tours.Count)
        {
            OnAllToursCompleted?.Invoke();
            Reset();
            return;
        }

        // Handle new tour
        if (CurrentTour != null)
        {
            CurrentTour.OnTourCompleted.AddListener(OnTourCompleted);
            CurrentTour.Activate();
            CurrentTour.StartTour();
            OnTourStarted?.Invoke();
        }

        OnTourChanged?.Invoke();
    }

    void ResetTours()
    {
        foreach (Tour tour in _tours)
            if (tour != null) tour.Reset();
    }
    #endregion

    #region EVENT METHODS
    void OnTourCompleted()
    {
        NextTour();
    }
    #endregion
}

