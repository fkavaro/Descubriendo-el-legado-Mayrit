using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class TutorialManager : ABehaviourEntity<StackFiniteStateMachine<TutorialState>>
{
    #region EDITOR PROPERTIES
    [Header("Tutorial Settings")]
    [SerializeField] UIManager _uiManager;
    [SerializeField] int _currentStepIndex = 0;
    [SerializeField] List<TutorialStepSO> _tutorialStepsData = new();
    #endregion

    #region INTERNAL PROPERTIES
    StackFiniteStateMachine<TutorialState> _fsm;
    ScenesController _scenesController;
    #endregion

    #region INHERITED
    public override StackFiniteStateMachine<TutorialState> DefineBehaviourSystemOnAwake()
    {
        _fsm = new(this);

        foreach (var data in _tutorialStepsData)
        {
            TutorialState newState = new(data, _uiManager, _fsm);
            _fsm.AddStateToSequence(newState);
            newState.AwakeState();
        }

        _fsm.SwitchedStateEvent += OnSwitchedState;
        _currentStepIndex = 0;
        _fsm.SetInitialStateFromSequence(_currentStepIndex);

        return _fsm;
    }
    #endregion

    #region LIFE CYCLE
    protected override void Awake()
    {
        ServiceLocator.Instance.Register(this);

        base.Awake();
    }

    protected override void Start()
    {
        _scenesController = ServiceLocator.Instance.Get<ScenesController>();
        _scenesController.ScenesLoadedFullyEvent += OnScenesLoadedFully;

        // base.Start(); when gameplay scene loaded, to start behaviour system
    }

    void OnDisable()
    {
        if (_scenesController != null)
            _scenesController.ScenesLoadedFullyEvent -= OnScenesLoadedFully;

        ServiceLocator.Instance.Unregister(this);
    }
    #endregion

    #region CALLBACK METHODS
    void OnScenesLoadedFully(Dictionary<SceneDatabase.SceneType, SceneDatabase.SceneName> loadedScenes, List<SceneDatabase.SceneType> unloadedTypes)
    {
        if (!loadedScenes.TryGetValue(SceneDatabase.SceneType.Milestone, out var milestoneScene))
            return;

        base.Start();
    }

    private void OnSwitchedState()
    {
        _currentStepIndex = (_currentStepIndex + 1) % _tutorialStepsData.Count;
    }
    #endregion


}
