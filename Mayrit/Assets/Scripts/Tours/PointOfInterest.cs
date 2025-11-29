using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PointOfInterest : MonoBehaviour
{
    #region PROPERTY HELPERS
    public DataSO Data => _data;
    public CinemachineCamera Camera => _camera;
    #endregion

    #region EDITOR PROPERTIES
    [Tooltip("Information associated with this POI")]
    [SerializeField] DataSO _data;

    [Header("Detection Settings")]
    [SerializeField] bool _isVisited;
    [Tooltip("Layer mask used for trigger checks (defaults to PlayableCharacter layer if present)")]
    [SerializeField] LayerMask _detectionMask = ~0;
    [SerializeField] float _colliderRadius = 2f;

    [Header("Camera")]
    [SerializeField] CinemachineCamera _camera;
    #endregion

    #region INTERNAL PROPERTIES
    public event Action<PointOfInterest> OnVisitedEvent;
    SphereCollider _sphereCollider;
    #endregion

    #region LIFE CYCLE
    /// <summary>
    /// Sets the collider radius and trigger settings.
    /// </summary>
    private void Awake()
    {
        if (TryGetComponent(out _sphereCollider))
        {
            _sphereCollider.radius = _colliderRadius;
            _sphereCollider.isTrigger = true;
        }

        // If detectionMask is left as default (all bits) and there's a layer named "PlayableCharacter",
        // restrict detection to that layer automatically so POIs only respond to the player.
        if (_detectionMask == (LayerMask)~0)
        {
            int playableLayer = LayerMask.NameToLayer("PlayableCharacter");
            if (playableLayer != -1)
                _detectionMask = 1 << playableLayer;
        }

        // Disable camera at start
        if (_camera == null)
        {
            Debug.LogWarning($"POI '{name}' has no Cinemachine camera assigned.");
            return;
        }

        _camera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when another collider enters the POI trigger zone. If the collider is on a valid layer and the POI
    /// hasn't been visited yet, marks it as visited.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (_isVisited) return;

        // Check layer mask
        if (((1 << other.gameObject.layer) & _detectionMask) == 0) return;

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
        OnVisitedEvent?.Invoke(this);
    }
    #endregion

    #region DEBUG GIZMOW
    void OnDrawGizmos()
    {
        Gizmos.color = _isVisited ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, _colliderRadius);

#if UNITY_EDITOR
        if (_data != null)
            UnityEditor.Handles.Label(transform.position + Vector3.up * (_colliderRadius + 0.2f),
            string.IsNullOrEmpty(_data.Header) ? name : _data.Header);
#endif
    }
    #endregion
}

