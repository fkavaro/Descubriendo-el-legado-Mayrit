using System;
using UnityEngine;
using UnityEngine.Events;
public class TourManager : MonoBehaviour
{
    [Tooltip("Player transform used to check POI visits")]
    public Transform _player;

    [Header("Visit Detection")]
    [Tooltip("If true, TourManager will use a layer-based physics check to detect visits instead of relying on POI.CheckVisited(actor).")]
    public bool useLayerDetection = false;

    [Tooltip("Layer mask used when useLayerDetection is enabled. If left as default and a layer named 'PlayableCharacter' exists, it will be used.")]
    public LayerMask visitingMask = ~0;

    [Tooltip("How to treat trigger colliders for manager overlap checks")]
    public QueryTriggerInteraction visitingTriggerInteraction = QueryTriggerInteraction.Ignore;

    [Tooltip("Internal buffer size for overlap checks (per-class shared buffer)")]
    public int visitingOverlapBufferSize = 8;

    // shared buffer used by manager for overlap checks
    private static Collider[] s_visitBuffer;
    private static readonly object s_visitLock = new object();

    [Tooltip("Optional starting tour (drag a GameObject with a Tour component)")]
    public Tour _startingTour;

    [Tooltip("Check every N seconds for arrival. Lower = more responsive")]
    public float _checkInterval = 0.25f;

    public UnityEvent _onTourStarted;
    public UnityEvent _onTourCompleted;
    public UnityEvent _onPOIChanged;

    private Tour _currentTour;
    private int _currentIndex = -1;
    private float _checkTimer;

    public PointOfInterest CurrentPOI =>
        (_currentTour != null && _currentIndex >= 0 && _currentIndex < _currentTour._pointsOfInterest.Count) ?
        _currentTour._pointsOfInterest[_currentIndex] :
        null;

    void Start()
    {
        if (_startingTour != null) StartTour(_startingTour);
    }

    void Awake()
    {
        // If visitingMask left as default and there's a PlayableCharacter layer, restrict automatically
        if (visitingMask == (LayerMask)~0)
        {
            int playable = LayerMask.NameToLayer("PlayableCharacter");
            if (playable != -1) visitingMask = 1 << playable;
        }
    }

    void OnValidate()
    {
        if (visitingOverlapBufferSize < 1) visitingOverlapBufferSize = 1;
    }

    void Update()
    {
        if (_currentTour == null || _player == null) return;
        _checkTimer -= Time.deltaTime;
        if (_checkTimer <= 0f)
        {
            _checkTimer = _checkInterval;
            var poi = CurrentPOI;
            if (poi != null)
            {
                bool visited = false;
                if (useLayerDetection)
                {
                    visited = ManagerPhysicsCheckVisit(poi);
                }
                else
                {
                    visited = poi.CheckVisited(_player);
                }

                if (visited)
                {
                    Advance();
                }
            }
        }
    }

    void StartTour(Tour tour)
    {
        if (tour == null) return;
        _currentTour = tour;
        _currentTour.ResetVisited();
        _currentIndex = -1;
        Next();
        _onTourStarted?.Invoke();
    }

    void Next()
    {
        if (_currentTour == null) return;
        _currentIndex++;
        if (_currentIndex >= _currentTour._pointsOfInterest.Count)
        {
            // finished
            _onTourCompleted?.Invoke();
            _currentIndex = -1;
            return;
        }
        _onPOIChanged?.Invoke();
    }

    void Advance()
    {
        Next();
    }

    void CancelTour()
    {
        _currentTour = null;
        _currentIndex = -1;
    }

    private bool ManagerPhysicsCheckVisit(PointOfInterest poi)
    {
        if (poi == null) return false;

        // Ensure shared buffer large enough
        EnsureVisitBufferSize(Mathf.Max(1, visitingOverlapBufferSize));

        int found = Physics.OverlapSphereNonAlloc(poi.transform.position, poi._visitRadius, s_visitBuffer, visitingMask, visitingTriggerInteraction);

        for (int i = 0; i < found; ++i)
        {
            var col = s_visitBuffer[i];
            if (col == null) continue;
            // similar logic to POI: prefer rigidbody transform
            var colTransform = col.attachedRigidbody != null ? col.attachedRigidbody.transform : col.transform;
            // we don't require exact actor transform here; any collider in mask counts
            if (colTransform != null)
            {
                poi.isVisited = true;
                poi.onVisited?.Invoke();
                return true;
            }
        }

        return false;
    }

    private void EnsureVisitBufferSize(int size)
    {
        if (s_visitBuffer != null && s_visitBuffer.Length >= size) return;
        lock (s_visitLock)
        {
            if (s_visitBuffer == null) s_visitBuffer = new Collider[size];
            else if (s_visitBuffer.Length < size) s_visitBuffer = new Collider[size];
        }
    }
}

