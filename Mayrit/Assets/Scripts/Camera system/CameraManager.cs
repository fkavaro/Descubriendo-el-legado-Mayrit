using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    #region PUBLIC PROPERTIES

    // Finite State Machine
    FiniteStateMachine<CameraManager> _fsm;
    Spectator_CameraState _spectatorState;

    [Header("Spectator camera")]
    public GameObject _spectatorCamera;
    public GameObject _spectatorCameraTarget;
    public CinemachineOrbitalFollow _orbitalFollow;
    [Tooltip("Wether to move camera at screen margins or not.")]
    public bool _edgeScrolling = false;
    public int _edgeScrollingMargin = 30;
    public float _moveSpeed = 500f;
    public AnimationCurve _moveSpeedZoomCurve = AnimationCurve.Linear(0f, 0.1f, 1f, 1f);
    public float _acceleration = 200f;
    public float _deceleration = 250f;
    public float _printSpeedMultiplier = 2f;
    [Tooltip("Mouse sensitivity for camera rotation.")]
    public float _orbitSensitivity = 0.5f;
    public float _orbitSmoothing = 5f;
    [Tooltip("Speed of camera zoom with scroll wheel.")]
    public float _zoomSpeed = 0.1f;
    public float _zoomSmoothing = 5f;

    [Header("Movement Limits")]
    [Tooltip("Maximum allowed X, Y, Z positions (positive and negative) for the camera.")]
    public Vector3 _movementLimits = new(800, 0, 800);


    [Header("Third Person Camera")]
    public GameObject _thirdPersonCamera;
    #endregion

    #region PRIVATE PROPERTIES
    #endregion

    #region INHERITED PROPERTIES
    protected override void OnAwake()
    {

    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }

    protected override ADecisionSystem<CameraManager> CreateDecisionSystem()
    {
        _fsm = new(this);

        _spectatorState = new(_fsm,
            _spectatorCamera.transform,
            _spectatorCameraTarget.transform,
            _orbitalFollow,
            _moveSpeedZoomCurve);

        _fsm.SetInitialState(_spectatorState);

        return _fsm;
    }

    internal void PlayPlayer()
    {
        _spectatorCamera.SetActive(false);
        _thirdPersonCamera.SetActive(true);
    }
    #endregion

    #region PUBLIC METHODS
    #endregion

    #region PRIVATE METHODS
    #endregion
}
