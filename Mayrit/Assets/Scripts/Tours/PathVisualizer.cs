using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [Tooltip("TourManager to read player and current POI from. If left empty, will try to find one on the same GameObject.")]
    public TourManager _tourManager;

    [Tooltip("LineRenderer to use. If empty, a LineRenderer will be added to this GameObject automatically.")]
    public LineRenderer _lineRenderer;

    [Tooltip("Use NavMesh.CalculatePath when available; otherwise fall back to a straight-line path.")]
    public bool _useNavMesh = true;

    [Tooltip("Seconds between path updates")]
    public float _updateInterval = 0.25f;

    [Tooltip("Width of the line")]
    public float _lineWidth = 0.12f;

    [Tooltip("Maximum number of corners to display (safety cap)")]
    public int _maxCorners = 512;

    [Tooltip("If true the LineRenderer will be enabled only when there is a target")]
    public bool _hideWhenNoTarget = true;

    public Gradient _colorGradient;
    #endregion

    #region MONOBEHAVIOUR
    void Awake()
    {
        _tourManager = TourManager.Instance;
        TryGetComponent(out _lineRenderer);
        ConfigureLineRenderer();
    }

    void Update()
    {

        if (_tourManager == null || _tourManager._player == null)
        {
            Clear();
            return;
        }

        var poi = _tourManager.CurrentTour.CurrentPOI;
        if (poi == null)
        {
            if (_hideWhenNoTarget) Clear();
            return;
        }

        DrawPath(_tourManager._player.position, poi.transform.position);
    }

    void OnEnable()
    {
        if (_tourManager != null)
            _tourManager.CurrentTour.OnPOIChanged.AddListener(OnPOIChanged);
    }

    void OnDisable()
    {
        if (_tourManager != null)
            _tourManager.CurrentTour.OnPOIChanged.RemoveListener(OnPOIChanged);

        Clear();
    }
    #endregion

    #region PUBLIC METHODS
    public void Clear()
    {
        if (_lineRenderer == null) return;
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
    }
    #endregion

    #region PRIVATE METHODS
    void ConfigureLineRenderer()
    {
        if (_lineRenderer == null) return;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.widthMultiplier = _lineWidth;
        if (_colorGradient == null)
        {
            _colorGradient = new Gradient();
            _colorGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0.9f, 1f) }
            );
        }
        _lineRenderer.colorGradient = _colorGradient;
        if (_lineRenderer.material == null) _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void DrawPath(Vector3 start, Vector3 end)
    {
        List<Vector3> corners = new List<Vector3>();

        if (_useNavMesh)
        {
            var path = new NavMeshPath();
            if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path) && path.corners != null && path.corners.Length > 0)
            {
                corners.AddRange(path.corners);
            }
        }

        if (corners.Count == 0)
        {
            corners.Add(start);
            corners.Add(end);
        }

        // Safety cap
        if (corners.Count > _maxCorners)
        {
            corners.RemoveRange(_maxCorners, corners.Count - _maxCorners);
        }

        if (_lineRenderer == null) return;
        _lineRenderer.positionCount = corners.Count;
        for (int i = 0; i < corners.Count; ++i)
            _lineRenderer.SetPosition(i, corners[i]);

        _lineRenderer.enabled = true;
    }
    #endregion

    #region EVENT METHODS
    void OnPOIChanged()
    {

    }
    #endregion
}
