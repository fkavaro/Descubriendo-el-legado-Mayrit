using System;
using UnityEngine;

public class ModernSuperposition : Singleton<ModernSuperposition>
{
    bool _isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetChildrenActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMode()
    {
        _isActive = !_isActive;
        SetChildrenActive(_isActive);
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(isActive);
    }
}
