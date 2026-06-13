using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class AHUDState : AUIState
{
    #region PROPERTIES
    protected CompassComponent _compass;

    VisualElement _hudScreen,
        _controlsMappingRoot,
        _compassVisualRoot;


    protected TutorialManager _tutorialManager;
    #endregion

    #region CONSTRUCTOR
    public AHUDState(UISystem uiSystem, string name, UIDocument uiDocument, float fadeInDuration, float fadeOutDuration)
    : base(uiSystem, name, uiDocument, fadeInDuration, fadeOutDuration) { }
    #endregion

    #region INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _controlsMappingRoot = GetByName<VisualElement>("ControlsMapping");
        _hudScreen = GetByName<VisualElement>("HUD", _UIDocument.rootVisualElement);
        _compassVisualRoot = GetByName<VisualElement>("Compass", _hudScreen);

        _compass = new(_uiSystem, _UIDocument, _compassVisualRoot);
        _compass.AwakeState();
        _hudScreen.style.display = DisplayStyle.None;
    }

    protected override void GetServicesDependenciesOnStart()
    {
        base.GetServicesDependenciesOnStart();

        if (_tutorialManager == null)
            _tutorialManager = ServiceLocator.Instance.Get<TutorialManager>();
    }

    public override void StartState()
    {
        _hudScreen.style.display = DisplayStyle.Flex;

        base.StartState();
        _compass.StartState();

        // Show controls visual according to UISystem setting
        _controlsMappingRoot.style.display = _gameManager.AreControlsMappingsDisplayed && _tutorialManager.HasCompletedTutorial ?
            DisplayStyle.Flex :
            DisplayStyle.None;

        _compass.IsShown = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _compass.UpdateState();
    }

    public override void ExitState()
    {
        _hudScreen.style.display = DisplayStyle.None;

        base.ExitState();
        _compass.IsShown = false;
    }
    #endregion
}
