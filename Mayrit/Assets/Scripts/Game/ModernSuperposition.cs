using System;
using UnityEngine;

public class ModernSuperposition : Singleton<ModernSuperposition>
{
    [Header("Settings")]
    public bool _isActive = false;

    bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            SetChildrenActive(_isActive);
        }
    }

    void Start()
    {
        IsActive = false;
        CameraManager.Instance.OnCameraStateChanged += CheckCameraState;
    }

    public void ToggleMode()
    {
        IsActive = !IsActive;
    }

    void CheckCameraState()
    {
        if (CameraManager.Instance._thirdPersonState.IsCurrentState())
            IsActive = false;
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isActive);
    }
}
