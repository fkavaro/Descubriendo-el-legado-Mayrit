using System;
using UnityEngine;

public abstract class ACharacter<BehaviourSystemType> : ABehaviourEntity<BehaviourSystemType>, ICharacter
where BehaviourSystemType : ABehaviourSystem
{
    #region PROPERTIES HELPERS
    public Animator CharacterAnimator => _animator;
    public CharacterAnimationController AnimationController => _animationController;

    public GameObject CharacterModel => _characterModel;
    public bool IsFemale => _gender == ICharacter.CharacterGender.Female;
    public string GivenName => _givenName;
    public string FamilyName => _familyName;

    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float RotationSpeed
    {
        get => _rotationSpeed;
        set => _rotationSpeed = value;
    }
    public float JumpForce => _jumpForce;
    public float GravityForce => _gravityForce;
    public float ArrivingDistance => _arrivingDistance;
    public float NearDistance => _nearDistance;
    public float FarDistance => _farDistance;
    #endregion

    #region EDITOR PROPERTIES
    [Header("Movement")]
    [SerializeField] protected float _walkSpeed = 1f;
    [SerializeField] protected float _sprintSpeed = 3f;
    [SerializeField] protected float _rotationSpeed = 4f;
    [SerializeField] protected float _jumpForce = 2f;
    [SerializeField] protected float _gravityForce = 9f;
    [SerializeField] protected float _arrivingDistance = 0.2f;
    [SerializeField] protected float _nearDistance = 2f;
    [SerializeField] protected float _farDistance = 15f;

    [Header("Animation")]
    [SerializeField] protected Animator _animator;

    [Header("Interaction")]
    [Tooltip("Minimum range to start an interaction with another character")]
    [SerializeField] protected float _interactionRange = 5f;

    [Header("Character information")]
    [SerializeField] protected ICharacter.CharacterGender _gender = ICharacter.CharacterGender.Male;
    [SerializeField] protected GameObject _characterModel;
    [SerializeField] protected string _givenName = "";
    [SerializeField] protected string _familyName = "";
    #endregion

    #region INTERNAL PROPERTIES   
    protected CharacterAnimationController _animationController;
    #endregion

    #region CHARACTER INFORMATION METHODS
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
    #endregion
}
