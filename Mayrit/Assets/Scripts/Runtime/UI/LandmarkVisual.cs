using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LandmarkVisual : Billboard
{
    #region EDITOR PROPERTIES
    [Header("Landmark information")]
    [SerializeField] bool _hideIfTooFar = true;
    [SerializeField] OrbitalStateSetting _orbitalStateSetting;
    [Header("Height Adjustment")]
    [SerializeField] bool _fixHeight = false;
    [SerializeField] AnimationCurve _heightMultiplierCurve;
    [SerializeField] float _cameraDistance;
    [SerializeField] float _currentHeightOffset;
    #endregion

    #region INTERNAL PROPERTIES
    Label _nameLabel;
    Button _nameButton;
    Vector3 _originalPosition;
    float OriginalHeight => _originalPosition.y;

    // Computed properties
    float CameraDistance
    {
        get
        {
            return Vector3.Distance(_originalPosition, _mainCamera.transform.position);
        }
    }


    // Dependency Injection
    UIManager _uiManager;
    SoundManager _soundManager;
    CameraManager _cameraManager;
    #endregion

    #region LIFE CYCLE
    void OnEnable()
    {
        if (_orbitalStateSetting.DataToShow == null)
        {
            Debug.LogWarning($"[LandmarkVisual] No information assigned to {gameObject.name}. Please assign a DataSO with the landmark's information.", this);
            return;
        }

        if (_orbitalStateSetting.Target == null)
            _orbitalStateSetting.Target = transform; // Default to self if no target assigned

        _originalPosition = transform.position;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _nameLabel = root.Q<Label>("Name");
        _nameButton = root.Q<Button>("NameButton");

        if (_nameLabel == null)
        {
            Debug.LogWarning("[LandmarkVisual] No Label with name 'Name' was found in the UIDocument.", this);
            return;
        }

        if (_nameButton == null)
        {
            Debug.LogWarning("[LandmarkVisual] No Button with name 'NameButton' was found in the UIDocument., this");
            return;
        }

        _nameLabel.text = _orbitalStateSetting.DataToShow.Header;
        _nameButton.RegisterCallback<ClickEvent>(OnNameButtonClick);
    }

    void Start()
    {
        // Get dependency from Service Locator
        _uiManager = ServiceLocator.Instance.Get<UIManager>();
        _soundManager = ServiceLocator.Instance.Get<SoundManager>();
        _cameraManager = ServiceLocator.Instance.Get<CameraManager>();

        _cameraManager.CameraStateChangedEvent += OnCameraStateChanged;
    }

    protected override void Update()
    {
        base.Update();

        if (_hideIfTooFar)
        {
            if (_isTooFar)
            {
                if (_nameButton.visible)
                    _nameButton.visible = false;
            }
            else
            {
                if (!_nameButton.visible)
                    _nameButton.visible = true;
            }
        }

        if (!_fixHeight) return;

        _currentHeightOffset = _heightMultiplierCurve.Evaluate(CameraDistance);
        _cameraDistance = CameraDistance;

        transform.position = new Vector3(transform.position.x, OriginalHeight + _currentHeightOffset, transform.position.z);
    }

    void OnDisable()
    {
        _nameButton?.UnregisterCallback<ClickEvent>(OnNameButtonClick);

        if (_cameraManager != null)
            _cameraManager.CameraStateChangedEvent -= OnCameraStateChanged;
    }
    #endregion

    #region CALLBACK METHODS
    void OnNameButtonClick(ClickEvent evt)
    {
        _uiManager.ShowContextualPanel(_orbitalStateSetting.DataToShow);
        _soundManager.PlayButtonClickSFX();

        if (_orbitalStateSetting.Target == null)
        {
            Debug.LogWarning($"[LandmarkVisual] can't orbit around null target.", this);
            return;
        }

        _cameraManager.SwitchToOrbitalCamera(_orbitalStateSetting);
    }

    void OnCameraStateChanged()
    {
        if (_cameraManager.IsInSpectatorState)
            _nameButton.visible = true;
        else
            _nameButton.visible = false;
    }
    #endregion
}
