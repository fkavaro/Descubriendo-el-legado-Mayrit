using UnityEngine;

[System.Serializable]
public class OrbitalCameraValues
{
    #region PROPERTY HELPERS
    public float OrbitSpeed => _orbitSpeed;
    public float ZoomValue => _zoomValue;
    public float HorizontalOffset => _horizontalOffset;
    #endregion

    #region EDITOR PROPERTIES
    [SerializeField] float _orbitSpeed = 10f;
    [SerializeField] float _zoomValue = 70f;
    [SerializeField] float _horizontalOffset = 20f;
    #endregion
}
