using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract base class for NPC (Non-Playable Character).
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class ANPC<T> : ABehaviourEntity<T>, INPC
where T : ABehaviourSystem
{
    #region EDITOR PROPERTIES
    [Header("Movement settings")]
    [Tooltip("Walking speed of the agent")]
    public float _walkSpeed = 2f;
    [Tooltip("Sprinting speed of the agent")]
    public float _sprintSpeed = 3f;
    [Tooltip("Rotation speed of the agent")]
    public float _rotationSpeed = 3f;
    [Tooltip("Distance to which the agent will avoid other agents"), Range(0.5f, 2f)]
    public float _avoidanceRadius = 0.7f;
    [Tooltip("Max distance from the random point to a point on the navmesh, for target position sampling")]
    public float _maxSamplingDistance = 1f;
    [Tooltip("Distance to which it's considered as arrived at destination (horizontal, vertical)")]
    public Vector2 _arrivedDistance = new(0.3f, 1.5f);
    public Vector2 _nearDistance = new(5f, 7f);
    public bool _isStopped = false;

    [Header("Avoidance settings")]
    [Tooltip("Base avoidance priority (0 = most important, 99 = least)")]
    public int _baseAvoidancePriority = 50;
    [Tooltip("Random +/- variance applied to base avoidance priority")]
    public int _avoidancePriorityVariance = 10;

    [Header("Animation settings")]
    public Animator _animator;

    [Header("Identity settings")]
    public GameObject _model;
    [SerializeField] string _givenName = "";
    [SerializeField] string _familyName = "";
    [SerializeField] INPC.NPCGender _gender = INPC.NPCGender.Male;

    [Header("Interaction settings")]
    [Tooltip("Cooldown time between interactions with other villlagers")]
    public float _interactionCooldown = 120f;
    [Tooltip("Maximum distance at which villagers consider others 'nearby' and can start an interaction")]
    public float _interactionRange = 3f;
    #endregion

    #region INTERNAL PROPERTIES
    NPCMovementController _movementController;
    NavMeshAgent _agent;
    CharacterAnimationController _animationController;
    bool _isInStreet = true;
    bool _isInteracting = false; // availability flag
    bool _wasAgentStoppedBeforeInteraction = false; // Keep previous agent stopped state if needed
    protected INPC _interactionTarget; // Cached target found by proximity queries to use for interactions
    #endregion

    #region PROPERTIES HELPERS
    public NavMeshAgent Agent => _agent;
    public CharacterAnimationController AnimationController => _animationController;

    public NPCMovementController MovementController => _movementController;
    public float WalkSpeed => _walkSpeed;
    public float RotationSpeed => _rotationSpeed;
    public Vector2 ArrivedDistance => _arrivedDistance;
    public Vector2 NearDistance => _nearDistance;
    public float AvoidanceRadius => _avoidanceRadius;
    public float MaxSamplingDistance => _maxSamplingDistance;
    public int AvoidancePriorityVariance => _avoidancePriorityVariance;
    public int BaseAvoidancePriority => _baseAvoidancePriority;
    public bool IsStopped
    {
        get => _isStopped;
        set => _isStopped = value;
    }

    public string GivenName => _givenName;
    public string FamilyName => _familyName;
    public INPC.NPCGender Gender => _gender;
    public bool IsFemale => _gender == INPC.NPCGender.Female;

    public bool IsInStreet
    {
        get => _isInStreet;
        set => _isInStreet = value;
    }
    public bool IsInteracting => _isInteracting;
    public INPC CurrentInteractionTarget => _interactionTarget;
    #endregion

    #region MONOBEHAVIOUR
    protected override void Awake()
    {
        base.Awake();

        _animationController = new(this, this, _animator);
        _agent = GetComponent<NavMeshAgent>();
        _movementController = new(this);
    }

    protected override void Update()
    {
        base.Update();

        _movementController.CheckNPCExecution();
    }
    #endregion

    #region PUBLIC METHODS
    public void SetFullName(string given, string family)
    {
        _givenName = given;
        _familyName = family;
        try
        {
            gameObject.name = string.IsNullOrEmpty(_familyName) ?
                _givenName :
                $"{_givenName} {_familyName}";
        }
        catch { }
    }

    public bool IsAvailableForInteraction()
    {
        return !_isInteracting && gameObject.activeInHierarchy;
    }

    public bool TryAcceptInteraction(INPC initiator)
    {
        if (!IsAvailableForInteraction())
            return false;

        Debug.Log($"{Name} accepted interaction with {initiator.Name}");

        _interactionTarget = initiator;
        StartInteraction();

        return true;
    }

    public void StartInteraction()
    {
        _isInteracting = true;

        // Keep previous logical stopped flag so we can restore it later
        _wasAgentStoppedBeforeInteraction = _isStopped;

        _movementController.SetIfStopped(true);
        AnimationController.ChangeToTalk();
    }

    public void EndInteraction()
    {
        _isInteracting = false;
        _interactionTarget = null;

        _movementController.SetIfStopped(_wasAgentStoppedBeforeInteraction);
        AnimationController.ChangeToWalk();
    }
    #endregion
}