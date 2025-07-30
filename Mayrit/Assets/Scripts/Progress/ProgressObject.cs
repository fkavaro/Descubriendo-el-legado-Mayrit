using System;
using UnityEngine;

public class ProgressObject : MonoBehaviour
{
    public ProgressManager.Milestone enablingMilestone;
    public ProgressManager.Milestone disablingMilestone;

    void Awake()
    {
        ProgressManager.Instance.OnMilestoneChanged += OnMilestoneChanged;
    }

    void Start()
    {

    }

    void OnMilestoneChanged(ProgressManager.Milestone entry)
    {
        if (enablingMilestone == entry)
            SetChildrenActive(true);
        else if (disablingMilestone == entry)
            SetChildrenActive(false);
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isActive);
    }
}
