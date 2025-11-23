using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
    [Tooltip("Ordered POIs for this tour (drag POI GameObjects)")]
    public List<PointOfInterest> _pointsOfInterest = new();

    public void ResetVisited()
    {
        foreach (var point in _pointsOfInterest)
        {
            if (point != null) point.isVisited = false;
        }
    }
}

