using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class PointOfInterest : MonoBehaviour
{
    #region EDITOR PROPERTIES
    [Tooltip("Invoke when this POI is visited")]
    public UnityEvent OnVisited;

    [Tooltip("Information associated with this POI")]
    public AInformationSO _information;

    [Tooltip("Radius around the POI where it counts as visited")]
    public float _visitRadius = 2f;

    [Tooltip("Layer mask used for trigger checks (defaults to PlayableCharacter layer if present)")]
    public LayerMask detectionMask = ~0;
    #endregion

    #region INTERNAL PROPERTIES
    bool _isVisited;
    SphereCollider _sphereCollider;
    #endregion

    #region MONOBEHAVIOUR
    /// <summary>
    /// Sets the collider radius and trigger settings.
    /// </summary>
    private void Awake()
    {
        if (_visitRadius < 0.5f) _visitRadius = 0.5f;
        if (TryGetComponent(out _sphereCollider))
        {
            _sphereCollider.radius = _visitRadius;
            _sphereCollider.isTrigger = true;
        }

        // If detectionMask is left as default (all bits) and there's a layer named "PlayableCharacter",
        // restrict detection to that layer automatically so POIs only respond to the player.
        if (detectionMask == (LayerMask)~0)
        {
            int playableLayer = LayerMask.NameToLayer("PlayableCharacter");
            if (playableLayer != -1)
            {
                detectionMask = 1 << playableLayer;
            }
        }
    }

    /// <summary>
    /// Called when another collider enters the POI trigger zone. If the collider is on a valid layer and the POI
    /// hasn't been visited yet, marks it as visited.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (_isVisited) return;

        // Check layer mask
        if (((1 << other.gameObject.layer) & detectionMask) == 0) return;

        SetAsVisited();
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Marks as unvisited and enables the POI collider.
    /// </summary>
    public void Activate()
    {
        _isVisited = false;
        _sphereCollider.enabled = true;
    }

    /// <summary>
    /// Marks as visited and disables the POI collider.
    /// </summary>
    public void Deactivate()
    {
        _isVisited = true;
        _sphereCollider.enabled = false;
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Marks this POI as visited and invokes the OnVisited event.
    /// </summary>
    void SetAsVisited()
    {
        if (_isVisited) return;

        _isVisited = true;
        OnVisited?.Invoke();
    }
    #endregion

    #region DEBUG GIZMOW
    void OnDrawGizmos()
    {
        Gizmos.color = _isVisited ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, _visitRadius);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * (_visitRadius + 0.2f),
            string.IsNullOrEmpty(_information.Header) ? name : _information.Header);
#endif
    }
    #endregion
}

