using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LandmarkVisual : Billboard
{
    #region EDITOR PROPERTIES
    [Header("Landmark information")]
    [SerializeField] OrbitalStateSetting _orbitalCameraValues;
    #endregion

    #region INTERNAL PROPERTIES
    UIDocument _uiDocument;
    Label _nameLabel;
    Button _nameButton;

    // Dependency Injection
    UIManager _uiManager;
    SoundManager _soundManager;
    CameraManager _cameraManager;
    #endregion

    #region LIFE CYCLE
    void OnEnable()
    {
        if (_orbitalCameraValues.DataToShow == null)
        {
            Debug.LogWarning($"[LandmarkVisual] No information assigned to {gameObject.name}. Please assign a DataSO with the landmark's information.", this);
            return;
        }

        if (_orbitalCameraValues.Target == null)
            _orbitalCameraValues.Target = transform; // Default to self if no target assigned

        // Try to get the UIDocument component from the same GameObject
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        _nameLabel = root.Q<Label>(name: "Name");
        _nameButton = root.Q<Button>(name: "NameButton");

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

        _nameLabel.text = _orbitalCameraValues.DataToShow.Header;
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
        _uiManager.ShowContextualPanel(_orbitalCameraValues.DataToShow);
        _soundManager.PlayButtonClickSFX();

        if (_orbitalCameraValues.Target == null)
        {
            Debug.LogWarning($"[LandmarkVisual] can't orbit around null target.", this);
            return;
        }

        _cameraManager.SwitchToOrbitalCamera(_orbitalCameraValues);
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
