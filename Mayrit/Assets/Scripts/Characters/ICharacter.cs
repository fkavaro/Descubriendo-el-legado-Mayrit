using System;
using UnityEngine;

public interface ICharacter : IBehaviourEntity
{
    public enum CharacterGender
    {
        Male,
        Female
    }

    #region PROPERTIES HELPERS
    public Animator CharacterAnimator { get; }
    public CharacterAnimationController AnimationController { get; set; }
    public GameObject CharacterModel { get; }
    public bool IsFemale { get; }
    float WalkSpeed { get; }
    float SprintSpeed { get; }
    float RotationSpeed { get; }
    float JumpForce { get; }
    float GravityForce { get; }
    float StoppingDistance { get; }
    float NearDistance { get; }
    #endregion
}
