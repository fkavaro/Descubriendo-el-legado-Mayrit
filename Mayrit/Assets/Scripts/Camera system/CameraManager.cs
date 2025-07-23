using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    #region PUBLIC PROPERTIES

    // Finite State Machine
    FiniteStateMachine<CameraManager> _fsm;
    Spectator_CameraState _spectatorState;
    ThirdPerson_CameraState _thirdPersonState;

    [Header("Spectator camera")]
    public CinemachineCamera _spectatorCamera;
    //public Transform _spectatorCameraTarget;
    public int _spectatorTargetHeight = 120;

    [Space]
    [Tooltip("Wether to move camera at screen margins or not.")]
    public bool _edgeScrolling = false;
    public int _edgeScrollingMargin = 30;

    [Space]
    public float _moveSpeed = 500f;
    public AnimationCurve _moveSpeedZoomCurve = AnimationCurve.Linear(0f, 0.1f, 1f, 1f);
    public float _acceleration = 200f;
    public float _deceleration = 250f;
    public float _printSpeedMultiplier = 2f;
    [Tooltip("Maximum allowed X, Y, Z positions (positive and negative) for the camera.")]
    public Vector3 _movementLimits = new(800, 0, 800);

    [Space]
    [Tooltip("Mouse sensitivity for camera rotation.")]
    public float _orbitSensitivity = 0.5f;
    public float _orbitSmoothing = 5f;

    [Space]
    [Tooltip("Speed of camera zoom with scroll wheel.")]
    public float _zoomSpeed = 0.1f;
    public float _zoomSmoothing = 5f;

    [Space]
    [Tooltip("Layer mask to define which objects are selectable.")]
    public LayerMask _selectableLayer;

    [Header("Third Person Camera")]
    public CinemachineCamera _thirdPersonCamera;

    [Header("Camera transition")]
    public float _transitionDuration = 1f;
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
            _spectatorCamera,
            _moveSpeedZoomCurve,
            _selectableLayer);

        _thirdPersonState = new(_fsm,
            _thirdPersonCamera);

        _fsm.SetInitialState(_spectatorState);

        return _fsm;
    }
    #endregion

    #region PUBLIC METHODS
    public void SwitchToThirdPersonCamera()
    {
        // Move spectator camera target smoothly to third person camera target
        StartCoroutine(SmoothMove(_spectatorCamera.LookAt,
            _thirdPersonCamera.LookAt.position,
            _transitionDuration,
            () =>
            {
                // Switch state when coroutine finished
                _fsm.SwitchState(_thirdPersonState);
            }
        ));
    }

    public void SwitchToSpectatorCamera()
    {
        _fsm.SwitchState(_spectatorState);

        // Fix player position for spectator camera
        Vector3 spectatorPlayerPos = new(
            _thirdPersonCamera.LookAt.position.x,
            _spectatorTargetHeight, // At spectator height
            _thirdPersonCamera.LookAt.position.z
        );

        // Move spectator camera target smoothly to fixed player position
        StartCoroutine(SmoothMove(_spectatorCamera.LookAt, // Will be at player position, from last transition
            spectatorPlayerPos,
            2f));
    }

    public void ToggleCameraState()
    {
        if (_fsm.IsCurrentState(_spectatorState))
            SwitchToThirdPersonCamera();
        else if (_fsm.IsCurrentState(_thirdPersonState))
            SwitchToSpectatorCamera();
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Moves smoothly the given transform to the new position in given duration.
    /// </summary>
    IEnumerator SmoothMove(Transform transform, Vector3 newPosition, float duration = 1f, Action onComplete = null)
    {
        if (transform == null || newPosition == null)
            yield break;

        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        // Duration remaining
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, newPosition, t);
            yield return null;
        }
        transform.position = newPosition;

        onComplete?.Invoke();
    }
    #endregion
}
