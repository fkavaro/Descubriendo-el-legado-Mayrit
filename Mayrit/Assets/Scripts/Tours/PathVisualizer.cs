using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Draws a navigation path between the player and the next Point Of Interest (POI).
///
/// Behaviour summary:
/// - Uses <see cref="NavMesh.CalculatePath"/> to obtain a navigation path (corners) on the active NavMesh.
/// - Samples along each corner segment at a configurable spacing and projects each sample
///   back onto the NavMesh to obtain correct surface heights.
/// - Optionally simplifies the sampled polyline using the Ramer–Douglas–Peucker algorithm
///   (to reduce vertex count while keeping overall shape).
/// - Sends the final polyline to a <see cref="LineRenderer"/> for display.
/// </summary>
public class PathVisualizer
{
    #region PROPERTIES
    // LineRenderer used for drawing the path. Must be supplied by the caller.
    readonly LineRenderer _lineRenderer;

    // Configuration fields (tweak these when creating the visualizer):
    // _sampleSpacing: distance between samples in metres along each path segment.
    // _sampleDistance: how far (metres) from a world position we'll look for a NavMesh hit
    //                 when snapping the start/end positions to the NavMesh.
    // _projSampleDistance: max distance used when projecting intermediate samples
    //                       to the NavMesh to obtain heights.
    // _renderYOffset: small vertical offset applied to rendered positions to avoid z-fighting.
    // _simplifyTolerance: epsilon (metres) used by Ramer–Douglas–Peucker; <= 0 disables.
    readonly float _sampleSpacing,
        _sampleDistance,
        _projSampleDistance,
        _renderYOffset,
        _simplifyTolerance;
    // Maximum number of points allowed to be sent to the LineRenderer. Acts as a safety cap.
    readonly int _maxPoints;
    // Whether to run the polyline simplification step.
    readonly bool _useSimplification;

    // Runtime state: current player transform and the next POI to point to.
    Transform _player, _nextPOI;
    Tour _currentTour;

    // Debug lists (populated after a path is sampled) used by editor gizmos.
    List<Vector3> _lastSampledPoints;
    List<Vector3> _lastSimplifiedPoints;
    public IReadOnlyList<Vector3> SampledPoints => _lastSampledPoints;
    public IReadOnlyList<Vector3> SimplifiedPoints => _lastSimplifiedPoints;
    #endregion

    #region CONSTRUCTOR
    public PathVisualizer(LineRenderer lineRenderer, float sampleSpacing, float sampleDistance, float projSampleDistance, float renderYOffset, int maxPoints, float simplifyTolerance, bool useSimplification)
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
    public void Initialize()
    {
        // Subscribe to milestone events so the visualizer can attach to the
        // currently active tour and receive player/POI updates.
        ProgressManager.Instance.OnMilestoneChangedEvent += OnMilestoneChanged;

        // Configure the LineRenderer for world-space drawing and hide it
        // until we have a valid path to show.
        if (_lineRenderer != null)
        {
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = false;
        }
    }

    public void UpdatePath()
    {
        // If no target POI or player is available, clear the line. This method
        // is called in Update() so it must be efficient to avoid hitches.
        if (_nextPOI == null || _player == null)
        {
            // Only log a warning if the line is currently visible to avoid
            // flooding the console every frame when the path is not available.
            if (_lineRenderer != null && _lineRenderer.enabled)
            {
                if (_nextPOI == null)
                    Debug.LogWarning("PathVisualizer: Clear path - no POI target");
                else if (_player == null)
                    Debug.LogWarning("PathVisualizer: Clear path - no playable character");
            }

            Clear();
            return;
        }

        // Compute and render a path from the player's position to the next POI.
        DrawPath(_player.position, _nextPOI.position);
    }

    public void Deinitialize()
    {
        // Unsubscribe from ProgressManager events and detach from the current
        // tour so there are no lingering subscriptions when the manager is destroyed.
        ProgressManager.ExistingInstance.OnMilestoneChangedEvent -= OnMilestoneChanged;

        DetachFromTour(_currentTour);
    }

    public void Clear()
    {
        if (_lineRenderer == null) return;
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
    }
    #endregion

    #region PRIVATE METHODS
    void DrawPath(Vector3 start, Vector3 end)
    {
        if (_lineRenderer == null) return;

        // Snap start/end to the NavMesh so paths adhere to the active NavMeshSurface
        bool hasStart = NavMesh.SamplePosition(start, out NavMeshHit startHit, _sampleDistance, NavMesh.AllAreas);
        bool hasEnd = NavMesh.SamplePosition(end, out NavMeshHit endHit, _sampleDistance, NavMesh.AllAreas);

        if (!hasStart || !hasEnd)
        {
            Debug.LogWarning("PathVisualizer: Start or end not on NavMesh (or too far). Clearing path.");
            Clear();
            return;
        }

        var navPath = new NavMeshPath();
        bool pathFound = NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, navPath);

        if (!pathFound || navPath.corners == null || navPath.corners.Length == 0)
        {
            Clear();
            return;
        }

        var baseCorners = navPath.corners;

        // Build a dense list of sample points along each straight corner-to-corner
        // segment. Each sample is projected back onto the NavMesh to obtain the
        // correct surface height (this is what makes the line follow terrain).
        var points = new List<Vector3>(baseCorners.Length * 2);

        // Reset debug arrays; they will be populated after sampling (and after
        // simplification if enabled).
        _lastSampledPoints = null;
        _lastSimplifiedPoints = null;

        for (int i = 0; i < baseCorners.Length - 1; i++)
        {
            Vector3 a = baseCorners[i];
            Vector3 b = baseCorners[i + 1];
            // Distance between corner points; determines how many samples we need
            float segLen = Vector3.Distance(a, b);
            // Number of discrete samples along this segment. Ceil ensures coverage.
            int steps = Mathf.Max(1, Mathf.CeilToInt(segLen / _sampleSpacing));

            for (int s = 0; s < steps; s++)
            {
                // Interpolate along the straight corner segment
                float t = (float)s / steps;
                Vector3 samplePoint = Vector3.Lerp(a, b, t);

                // Project the horizontal sample onto the NavMesh to pick up the
                // correct surface height at that position. If no hit is found
                // within `_projSampleDistance`, we keep the interpolated point.
                if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, _projSampleDistance, NavMesh.AllAreas))
                    samplePoint = hit.position;

                // Slight visual lift to avoid z-fighting with terrain geometry.
                samplePoint.y += _renderYOffset;

                // Avoid adding nearly-duplicate consecutive points.
                if (points.Count == 0 || (points[points.Count - 1] - samplePoint).sqrMagnitude > 0.0001f)
                    points.Add(samplePoint);
            }
        }

        // Add last corner (ensure it's projected)
        Vector3 last = baseCorners[^1];
        if (NavMesh.SamplePosition(last, out NavMeshHit lastHit, _projSampleDistance, NavMesh.AllAreas))
            last = lastHit.position;
        last.y += _renderYOffset;
        if (points.Count == 0 || (points[points.Count - 1] - last).sqrMagnitude > 0.0001f)
            points.Add(last);

        if (points.Count == 0)
        {
            Clear();
            return;
        }

        // Optional simplification step: run Ramer–Douglas–Peucker to reduce vertex
        // count while approximating the original polyline within `_simplifyTolerance`.
        // We keep copies of the raw sampled points and the simplified points for
        // debugging (gizmos) so you can compare the dense samples vs the result.
        if (_useSimplification && _simplifyTolerance > 0f && points.Count > 2)
        {
            _lastSampledPoints = new List<Vector3>(points);
            var simplified = RamerDouglasPeucker(points, _simplifyTolerance);
            points = simplified;
            _lastSimplifiedPoints = new List<Vector3>(points);
        }
        else
        {
            // No simplification performed — both debug lists contain the same samples.
            _lastSampledPoints = new List<Vector3>(points);
            _lastSimplifiedPoints = new List<Vector3>(points);
        }

        // Safety cap: if there are still too many points (very long path or
        // extremely dense sampling), uniformly downsample the polyline so we
        // do not exceed the LineRenderer's practical limit.
        if (points.Count > _maxPoints)
        {
            var sampled = new List<Vector3>(_maxPoints + 1);
            int step = Mathf.CeilToInt((float)points.Count / _maxPoints);
            for (int i = 0; i < points.Count; i += step)
                sampled.Add(points[i]);
            if (sampled[sampled.Count - 1] != points[points.Count - 1])
                sampled.Add(points[points.Count - 1]);
            points = sampled;
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
        _lineRenderer.enabled = true;
    }

    /// <summary>
    /// This recursive implementation preserves endpoints and removes internal
    /// vertices that deviate from the straight-line approximation by less than
    /// 'epsilon' metres. It is useful to reduce the number of vertices while
    /// maintaining the visual shape of the path.
    /// </summary>
    List<Vector3> RamerDouglasPeucker(List<Vector3> points, float epsilon)
    {
        if (points == null || points.Count < 3) return new List<Vector3>(points ?? new List<Vector3>());

        int index = -1;
        float maxDist = 0f;

        // Find the point with the maximum perpendicular distance to the line
        // connecting the first and last point.
        for (int i = 1; i < points.Count - 1; i++)
        {
            float dist = PerpendicularDistance(points[i], points[0], points[points.Count - 1]);
            if (dist > maxDist)
            {
                index = i;
                maxDist = dist;
            }
        }

        // If the maximum distance is greater than epsilon, recursively simplify
        // the left and right segments and combine the results (removing the
        // duplicated pivot point).
        if (maxDist > epsilon)
        {
            var left = RamerDouglasPeucker(points.GetRange(0, index + 1), epsilon);
            var right = RamerDouglasPeucker(points.GetRange(index, points.Count - index), epsilon);

            var result = new List<Vector3>(left.Count + right.Count - 1);
            result.AddRange(left);
            result.AddRange(right.GetRange(1, right.Count - 1));
            return result;
        }

        // Otherwise the segment is well approximated by a single straight line
        // between endpoints — return only the start and end points.
        return new List<Vector3> { points[0], points[points.Count - 1] };
    }


    /// <summary>
    /// Compute shortest distance from `point` to the finite segment
    /// [lineStart, lineEnd]. This clamps the projection to the segment
    /// endpoints so points near the ends are measured to the nearest
    /// endpoint rather than to the infinite line.
    /// </summary>
    static float PerpendicularDistance(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        var seg = lineEnd - lineStart;
        float segLenSq = seg.sqrMagnitude;
        if (segLenSq <= Mathf.Epsilon)
            return Vector3.Distance(point, lineStart);

        // Project point onto the segment, expressed as a normalized factor t
        // along the segment, then clamp to [0,1] to confine to the finite segment.
        float t = Vector3.Dot(point - lineStart, seg) / segLenSq;
        t = Mathf.Clamp01(t);
        var closest = lineStart + seg * t;
        return Vector3.Distance(point, closest);
    }

    void AttachToTour(Tour tour)
    {
        if (_currentTour == tour) return;

        DetachFromTour(_currentTour);

        _currentTour = tour;
        _currentTour.OnNextPOIChangeEvent += OnNextPOIChange;
    }

    void DetachFromTour(Tour tour)
    {
        if (tour == null) return;

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
