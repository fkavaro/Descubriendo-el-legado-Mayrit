using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ProgressObject listens to ProgressManager milestone changes and activates/deactivates
/// its child objects accordingly. This implementation is defensive for editor-time
/// operations (OnValidate, import callbacks) and avoids accessing singletons that may
/// not be initialized during editor import.
/// </summary>
public class ProgressObject : MonoBehaviour
{
    public List<int> milestonesActivated;

    #region MONOBEHAVIOUR
    void OnEnable()
    {
        SubscribeToRuntimeEvents();
    }

    void OnDisable()
    {
        UnsubscribeToRuntimeEvents();
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            SubscribeToRuntimeEvents();
#endif
    }
    #endregion

    #region PRIVATE METHODS
    void SetChildrenActive(bool isActive)
    {
        if (this == null) return;

        foreach (Transform child in transform)
        {
            if (child == null || child.gameObject == null) continue;
            if (child.gameObject.activeSelf != isActive)
                child.gameObject.SetActive(isActive);
        }
    }

    void SubscribeToRuntimeEvents()
    {
        ProgressManager progressManager = FindAnyObjectByType<ProgressManager>();

        if (progressManager != null)
        {
            progressManager.OnMilestoneChangedEvent += OnMilestoneChanged;
            progressManager.OnEditorUpdateChangedEvent += OnEditorUpdateChanged;
        }
    }

    void UnsubscribeToRuntimeEvents()
    {
        ProgressManager progressManager = FindAnyObjectByType<ProgressManager>();

        if (progressManager != null)
        {
            progressManager.OnMilestoneChangedEvent -= OnMilestoneChanged;
            progressManager.OnEditorUpdateChangedEvent -= OnEditorUpdateChanged;
        }
    }
    #endregion

    #region EVENTS METHODS
    void OnMilestoneChanged(int milestoneIndex)
    {
        if (this == null) return;

        SetChildrenActive(milestonesActivated.Contains(milestoneIndex));
    }

    void OnEditorUpdateChanged(bool updateInEditor)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            return;

        if (this == null) return;
        if (!updateInEditor)
            SetChildrenActive(true);
        else
        {
            var progressManager = FindAnyObjectByType<ProgressManager>();
            if (progressManager == null) return;
            var milestone = progressManager.CurrentMilestoneIndex;
            SetChildrenActive(milestonesActivated.Contains(milestone));
        }

#endif
    }
    #endregion
}
