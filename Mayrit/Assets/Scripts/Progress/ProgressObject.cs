using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressObject : MonoBehaviour
{
    public List<ProgressManager.Milestone> milestonesActivated;

    void Awake()
    {
        ProgressManager.Instance.OnMilestoneChanged += OnMilestoneChanged;
    }

    void OnValidate()
    {
        ProgressManager.Instance.OnMilestoneChanged += OnMilestoneChanged;
    }

    void OnMilestoneChanged(ProgressManager.Milestone entry)
    {
#if UNITY_EDITOR
        // Playing
        if (Application.isPlaying)
            // Active if entry milestone is in the list
            SetChildrenActive(milestonesActivated != null && milestonesActivated.Contains(entry));
        // Not playing
        else
        {
            // React to changes in editor
            if (ProgressManager.Instance._updateInInspector)
                // Active if entry milestone is in the list
                SetChildrenActive(milestonesActivated != null && milestonesActivated.Contains(entry));
            // Active if not updating in inspector
            else
                SetChildrenActive(true);
        }
#else
        // Active if entry milestone is in the list
        SetChildrenActive(milestonesActivated != null && milestonesActivated.Contains(entry));
#endif
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isActive);
    }
}
