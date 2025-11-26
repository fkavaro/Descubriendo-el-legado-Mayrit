using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Draws a navigation path between the player and the next Point Of Interest (POI) of the tour.
/// </summary>
public class PathVisualizer
{
    #region FIELDS
    // Configuration
    readonly LineRenderer _lineRenderer;
    readonly float _sampleSpacing;       // Distance between samples along segments
    readonly float _sampleDistance;      // Max distance to snap start/end to NavMesh
    readonly float _projSampleDistance;  // Max distance to project samples to NavMesh
    readonly float _renderYOffset;       // Vertical offset to lift the rendered line above navmesh
    readonly float _simplifyTolerance;   // RDP tolerance in metres; <=0 disables simplification
    readonly int _maxPoints;             // Maximum number of points allowed on the LineRenderer
    readonly bool _useSimplification;    // Whether to run the polyline simplification step

    // Runtime state
    Transform _player;
    Transform _nextPOI;
    Tour _currentTour;

    // Debug arrays (populated after sampling) used by editor gizmos
    List<Vector3> _lastSampledPoints;
    List<Vector3> _lastSimplifiedPoints;

    // Reusable buffer for RDP 'keep' markers to avoid per-call allocations.
    bool[] _rdpKeepBuffer;
    // Reusable stack for iterative RDP to avoid per-call allocations.
    Stack<int> _rdpStack;
    // Reusable result buffer for RDP output to avoid allocating a new list.
    List<Vector3> _rdpResultBuffer;

    public IReadOnlyList<Vector3> SampledPoints => _lastSampledPoints;
    public IReadOnlyList<Vector3> SimplifiedPoints => _lastSimplifiedPoints;
    #endregion

    #region CONSTRUCTOR
    public PathVisualizer(
        LineRenderer lineRenderer,
        float sampleSpacing,
        float sampleDistance,
        float projSampleDistance,
        float renderYOffset,
        int maxPoints,
        float simplifyTolerance,
        bool useSimplification)
    {
        _lineRenderer = lineRenderer;
        _sampleSpacing = Mathf.Max(0.01f, sampleSpacing);
        _sampleDistance = Mathf.Max(0.01f, sampleDistance);
        _projSampleDistance = Mathf.Max(0.01f, projSampleDistance);
        _renderYOffset = renderYOffset;
        _maxPoints = Mathf.Max(16, maxPoints);
        _simplifyTolerance = simplifyTolerance;
        _useSimplification = useSimplification;
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Subscribe to milestone events and prepare the LineRenderer.
    /// </summary>
    public void Initialize()
    {
        ProgressManager.Instance.OnMilestoneChangedEvent += OnMilestoneChanged;

        if (_lineRenderer == null)
            return;

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
    }

    /// <summary>
    /// Update the visualized path between player and next POI (if available).
    /// </summary>
    public void UpdatePath()
    {
        if (_nextPOI == null || _player == null)
        {
            if (_lineRenderer != null && _lineRenderer.enabled)
            {
                if (_nextPOI == null)
                    Debug.LogWarning("PathVisualizer: Clear path - no POI target");
                else
                    Debug.LogWarning("PathVisualizer: Clear path - no playable character");
            }

            Clear();
            return;
        }

        DrawPath(_player.position, _nextPOI.position);
    }

    /// <summary>
    /// Unsubscribe and detach from current tour.
    /// </summary>
    public void Deinitialize()
    {
        ProgressManager.ExistingInstance.OnMilestoneChangedEvent -= OnMilestoneChanged;
        DetachFromTour(_currentTour);
    }

    /// <summary>
    /// Clear the LineRenderer and hide it.
    /// </summary>
    public void Clear()
    {
        if (_lineRenderer == null)
            return;

        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
    }
    #endregion

    #region PATH SAMPLING
    void DrawPath(Vector3 start, Vector3 end)
    {
        if (_lineRenderer == null)
            return;

        // Snap start/end to the NavMesh so paths adhere to the active surface
        bool hasStart = NavMesh.SamplePosition(start, out NavMeshHit startHit, _sampleDistance, NavMesh.AllAreas);
        bool hasEnd = NavMesh.SamplePosition(end, out NavMeshHit endHit, _sampleDistance, NavMesh.AllAreas);

        if (!hasStart || !hasEnd)
        {
            Debug.LogWarning("PathVisualizer: Start or end not on NavMesh (or too far). Clearing path.");
            Clear();
            return;
        }

        NavMeshPath navPath = new();
        bool pathFound = NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, navPath);

        // Clear if no path found or no corners
        if (!pathFound || navPath.corners == null || navPath.corners.Length == 0)
        {
            Clear();
            return;
        }

        Vector3[] baseCorners = navPath.corners;

        // Build a dense list of sample points along each straight corner-to-corner segment.
        List<Vector3> points = new(baseCorners.Length * 2);

        // Reset debug arrays; they will be populated after sampling / simplification.
        _lastSampledPoints = null;
        _lastSimplifiedPoints = null;

        for (int i = 0; i < baseCorners.Length - 1; i++)
        {
            Vector3 a = baseCorners[i];
            Vector3 b = baseCorners[i + 1];
            float segLen = Vector3.Distance(a, b);
            int steps = Mathf.Max(1, Mathf.CeilToInt(segLen / _sampleSpacing));

            for (int s = 0; s < steps; s++)
            {
                float t = (float)s / steps;
                Vector3 samplePoint = Vector3.Lerp(a, b, t);

                if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, _projSampleDistance, NavMesh.AllAreas))
                    samplePoint = hit.position;

                samplePoint.y += _renderYOffset;

                if (points.Count == 0 || (points[^1] - samplePoint).sqrMagnitude > 0.0001f)
                    points.Add(samplePoint);
            }
        }

        // Add last corner (ensure projection)
        Vector3 last = baseCorners[^1];
        if (NavMesh.SamplePosition(last, out NavMeshHit lastHit, _projSampleDistance, NavMesh.AllAreas))
            last = lastHit.position;

        last.y += _renderYOffset;
        if (points.Count == 0 || (points[^1] - last).sqrMagnitude > 0.0001f)
            points.Add(last);

        if (points.Count == 0)
        {
            Clear();
            return;
        }

        // Optional simplification (Ramer–Douglas–Peucker)
        if (_useSimplification && _simplifyTolerance > 0f && points.Count > 2)
        {
            _lastSampledPoints = new List<Vector3>(points);
            List<Vector3> simplified = RamerDouglasPeucker(points, _simplifyTolerance);
            points = simplified;
            _lastSimplifiedPoints = new List<Vector3>(points);
        }
        else
        {
            _lastSampledPoints = new List<Vector3>(points);
            _lastSimplifiedPoints = new List<Vector3>(points);
        }

        // Safety cap: uniformly downsample if still too many points
        if (points.Count > _maxPoints)
        {
            List<Vector3> sampled = new(_maxPoints + 1);
            int step = Mathf.CeilToInt((float)points.Count / _maxPoints);
            for (int i = 0; i < points.Count; i += step)
                sampled.Add(points[i]);

            if (sampled[^1] != points[^1])
                sampled.Add(points[^1]);

            points = sampled;
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
        _lineRenderer.enabled = true;
    }
    #endregion

    #region RDP SIMPLIFICATION
    /// <summary>
    /// Iterative Ramer–Douglas–Peucker polyline simplification. Preserves endpoints.
    /// Implementation uses an explicit stack and index ranges to avoid
    /// repeated list copying and deep recursion. Distance checks use
    /// squared distances to avoid unnecessary sqrt operations.
    /// </summary>
    List<Vector3> RamerDouglasPeucker(List<Vector3> points, float epsilon)
    {
        if (points == null || points.Count < 3)
            return new List<Vector3>(points ?? new List<Vector3>());

        int n = points.Count;

        // Boolean marker for points to keep. Endpoints always kept.
        // Reuse a per-instance buffer when possible to avoid allocations.
        if (_rdpKeepBuffer == null || _rdpKeepBuffer.Length < n)
            _rdpKeepBuffer = new bool[n];
        else
            Array.Clear(_rdpKeepBuffer, 0, n);

        bool[] keep = _rdpKeepBuffer;
        keep[0] = true;
        keep[n - 1] = true;

        // Ensure a reusable Stack<int> exists and clear it for reuse. Using a
        // reused Stack preserves readability while avoiding per-call allocs.
        if (_rdpStack == null)
            _rdpStack = new Stack<int>(n * 2);
        else
            _rdpStack.Clear();

        _rdpStack.Push(0);
        _rdpStack.Push(n - 1);

        float epsSq = epsilon * epsilon;

        while (_rdpStack.Count > 0)
        {
            int end = _rdpStack.Pop();
            int start = _rdpStack.Pop();

            // Find the point with maximum perpendicular squared distance
            // to the segment [start, end].
            float maxDistSq = 0f;
            int pivot = -1;

            Vector3 A = points[start];
            Vector3 B = points[end];
            Vector3 seg = B - A;
            float segLenSq = seg.sqrMagnitude;

            for (int i = start + 1; i < end; i++)
            {
                float distSq;

                if (segLenSq <= Mathf.Epsilon)
                {
                    // Degenerate segment: distance to the start point.
                    distSq = (points[i] - A).sqrMagnitude;
                }
                else
                {
                    // Perpendicular squared distance using cross-product magnitude.
                    Vector3 v = points[i] - A;
                    Vector3 cross = Vector3.Cross(seg, v);
                    distSq = cross.sqrMagnitude / segLenSq;
                }

                if (distSq > maxDistSq)
                {
                    maxDistSq = distSq;
                    pivot = i;
                }
            }

            // If the maximum squared distance exceeds threshold, mark pivot
            // and split the segment for further processing.
            if (pivot != -1 && maxDistSq > epsSq)
            {
                keep[pivot] = true;
                // Push the two subsegments: [start,pivot] and [pivot,end]
                _rdpStack.Push(start);
                _rdpStack.Push(pivot);
                _rdpStack.Push(pivot);
                _rdpStack.Push(end);
            }
        }

        // Build result from kept points (preserves original order).
        if (_rdpResultBuffer == null)
            _rdpResultBuffer = new List<Vector3>(n);
        else
        {
            _rdpResultBuffer.Clear();
            if (_rdpResultBuffer.Capacity < n)
                _rdpResultBuffer.Capacity = n;
        }

        for (int i = 0; i < n; i++)
            if (keep[i])
                _rdpResultBuffer.Add(points[i]);

        return _rdpResultBuffer;
    }
    #endregion

    #region TOUR ATTACHMENT
    void AttachToTour(Tour tour)
    {
        if (_currentTour == tour)
            return;

        DetachFromTour(_currentTour);

        _currentTour = tour;
        _currentTour.OnNextPOIChangeEvent += OnNextPOIChange;
    }

    void DetachFromTour(Tour tour)
    {
        if (tour == null)
            return;

        tour.OnNextPOIChangeEvent -= OnNextPOIChange;
        _nextPOI = null;
        _currentTour = null;
    }
    #endregion

    #region EVENT METHODS
    void OnMilestoneChanged(MilestoneMapping milestoneMapping)
    {
        _player = milestoneMapping.PlayableCharacter.transform;
        AttachToTour(milestoneMapping.Tour);
    }

    void OnNextPOIChange(PointOfInterest poi)
    {
        _nextPOI = poi.transform;
    }
    #endregion
}
