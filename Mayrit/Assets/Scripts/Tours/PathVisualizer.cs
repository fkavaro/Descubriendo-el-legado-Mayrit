using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TourManager))]
[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    [Tooltip("TourManager to read player and current POI from. If left empty, will try to find one on the same GameObject.")]
    public TourManager manager;

    [Tooltip("LineRenderer to use. If empty, a LineRenderer will be added to this GameObject automatically.")]
    public LineRenderer lineRenderer;

    [Tooltip("Use NavMesh.CalculatePath when available; otherwise fall back to a straight-line path.")]
    public bool useNavMesh = true;

    [Tooltip("Seconds between path updates")]
    public float updateInterval = 0.25f;

    [Tooltip("Width of the line")]
    public float lineWidth = 0.12f;

    [Tooltip("Maximum number of corners to display (safety cap)")]
    public int maxCorners = 512;

    [Tooltip("If true the LineRenderer will be enabled only when there is a target")]
    public bool hideWhenNoTarget = true;

    public Gradient colorGradient;

    float timer;

    void Reset()
    {
        manager = GetComponent<TourManager>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        ConfigureLineRenderer();
    }

    void OnEnable()
    {
        if (manager == null) manager = GetComponent<TourManager>();
        if (manager != null) manager._onPOIChanged.AddListener(OnPOIChanged);
        timer = 0f;
    }

    void OnDisable()
    {
        if (manager != null) manager._onPOIChanged.RemoveListener(OnPOIChanged);
        Clear();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = updateInterval;

        if (manager == null || manager._player == null)
        {
            Clear();
            return;
        }

        var poi = manager.CurrentPOI;
        if (poi == null)
        {
            if (hideWhenNoTarget) Clear();
            return;
        }

        DrawPath(manager._player.position, poi.transform.position);
    }

    void OnPOIChanged()
    {
        timer = 0f; // immediate refresh when target changes
    }

    void ConfigureLineRenderer()
    {
        if (lineRenderer == null) return;
        lineRenderer.useWorldSpace = true;
        lineRenderer.widthMultiplier = lineWidth;
        if (colorGradient == null)
        {
            colorGradient = new Gradient();
            colorGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0.9f, 1f) }
            );
        }
        lineRenderer.colorGradient = colorGradient;
        if (lineRenderer.material == null) lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void DrawPath(Vector3 start, Vector3 end)
    {
        List<Vector3> corners = new List<Vector3>();

        if (useNavMesh)
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
        if (corners.Count > maxCorners)
        {
            corners.RemoveRange(maxCorners, corners.Count - maxCorners);
        }

        if (lineRenderer == null) return;
        lineRenderer.positionCount = corners.Count;
        for (int i = 0; i < corners.Count; ++i)
            lineRenderer.SetPosition(i, corners[i]);

        lineRenderer.enabled = true;
    }

    public void Clear()
    {
        if (lineRenderer == null) return;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }
}
