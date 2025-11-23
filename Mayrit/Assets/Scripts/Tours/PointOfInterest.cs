using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class PointOfInterest : MonoBehaviour
{
    [Tooltip("Unique id for the POI (optional)")]
    public string _POIid;

    [Tooltip("Short title shown in UI")]
    public AInformationSO _information;

    [Tooltip("Radius around the POI where it counts as visited")]
    public float _visitRadius = 2f;

    [Tooltip("Invoke when this POI is visited")]
    public UnityEvent onVisited;

    [HideInInspector]
    public bool isVisited;

    [Tooltip("Layer mask used for overlap checks (defaults to Everything)")]
    public LayerMask detectionMask = ~0;

    [Tooltip("How to treat trigger colliders during overlap checks")]
    public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Tooltip("Size of the internal overlap buffer (increase if you expect many colliders nearby)")]
    public int overlapBufferSize = 16;

    // shared internal reusable buffer (not serialized). Using a single static buffer reduces memory
    // usage when many POI instances exist. It will be resized as needed. Calls must process
    // results immediately because the buffer is shared.
    private static Collider[] s_overlapBuffer;
    private static readonly object s_overlapLock = new object();

    private void Awake()
    {
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

    private void OnValidate()
    {
        // Keep overlap buffer size sane
        if (overlapBufferSize < 1) overlapBufferSize = 1;
    }

    public bool CheckVisited(Transform actor)
    {
        if (isVisited || actor == null) return false;

        // Ensure shared buffer exists and is appropriately sized. We resize the shared buffer
        // under a lock to avoid races if multiple POIs are validated in the editor (or other unexpected cases).
        EnsureSharedBufferSize(Mathf.Max(1, overlapBufferSize));

        // Use Physics.OverlapSphereNonAlloc for low GC and fast checks
        int found = Physics.OverlapSphereNonAlloc(transform.position, _visitRadius, s_overlapBuffer, detectionMask, triggerInteraction);
        for (int i = 0; i < found; ++i)
        {
            var col = s_overlapBuffer[i];
            if (col == null) continue;

            // Prefer attachedRigidbody's transform (covers ragdolls / rigidbody-on-parent setups)
            Transform colTransform = col.attachedRigidbody != null ? col.attachedRigidbody.transform : col.transform;

            // Accept if the collider belongs to the actor or is a child/parent of it
            if (colTransform == actor || colTransform.IsChildOf(actor) || actor.IsChildOf(colTransform))
            {
                isVisited = true;
                onVisited?.Invoke();
                return true;
            }
        }

        return false;
    }

    private static void EnsureSharedBufferSize(int size)
    {
        // Quick path: already large enough
        if (s_overlapBuffer != null && s_overlapBuffer.Length >= size) return;

        lock (s_overlapLock)
        {
            if (s_overlapBuffer == null)
            {
                s_overlapBuffer = new Collider[size];
            }
            else if (s_overlapBuffer.Length < size)
            {
                // grow to required size (avoid shrinking)
                var newBuf = new Collider[size];
                // no need to copy contents because buffer is transient
                s_overlapBuffer = newBuf;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isVisited ? new Color(0.2f, 1f, 0.2f, 0.35f) : new Color(1f, 0.6f, 0.1f, 0.35f);
        Gizmos.DrawSphere(transform.position, _visitRadius);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * (_visitRadius + 0.2f),
            string.IsNullOrEmpty(_information.Header) ? name : _information.Header);
#endif
    }
}

