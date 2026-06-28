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
    public CharacterAnimationController AnimationController { get; }
    public GameObject CharacterModel { get; }
    public bool ShouldRenderCharacterModel { get; }
    public bool IsOutdoors { get; }
    public bool IsFemale { get; }
    public string GivenName { get; }
    public string FamilyName { get; }
    float WalkSpeed { get; }
    float SprintSpeed { get; }
    float RotationSpeed { get; set; }
    float JumpForce { get; }
    float GravityForce { get; }
    float ArrivingDistance { get; }
    float NearDistance { get; }
    float FarDistance { get; }
    float InteractionRange { get; }
    #endregion
    #region METHODS
    public void SetFullName(string given, string family);
    #endregion
}
