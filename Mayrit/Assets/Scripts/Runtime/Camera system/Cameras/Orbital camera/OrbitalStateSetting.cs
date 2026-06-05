using System;
using UnityEngine;

public enum CameraTransition
{
    AerialCamera,
    ThirdPersonCamera,
}

[Serializable]
public class OrbitalStateSetting
{
    public float OrbitSpeed => _orbitSpeed;
    public float ZoomValue => _zoomValue;
    public float HorizontalOffset => _horizontalOffset;

    public bool IsForCharacter = false;
    public DataSO DataToShow;
    public Transform Target;
    public CameraTransition TransitionToApply = CameraTransition.AerialCamera;
    [SerializeField] float _orbitSpeed = 10f;
    [SerializeField] float _zoomValue = 70f;
    [SerializeField] float _horizontalOffset = 20f;
}