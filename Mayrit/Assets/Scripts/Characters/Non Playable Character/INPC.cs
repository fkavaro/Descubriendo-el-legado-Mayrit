using UnityEngine;
using UnityEngine.AI;

public interface INPC : IBehaviourEntity
{
    public enum NPCGender
    {
        Male,
        Female
    }

    #region HELPER METHODS
    public NavMeshAgent Agent { get; }
    public CharacterAnimationController AnimationController { get; }

    NPCMovementController MovementController { get; }
    float WalkSpeed { get; }
    float RotationSpeed { get; }
    Vector2 ArrivedDistance { get; }
    Vector2 NearDistance { get; }
    float AvoidanceRadius { get; }
    float MaxSamplingDistance { get; }
    int AvoidancePriorityVariance { get; }
    int BaseAvoidancePriority { get; }
    bool IsStopped { get; set; }

    public string GivenName { get; }
    public string FamilyName { get; }
    public NPCGender Gender { get; }
    public bool IsFemale { get; }

    bool IsInStreet { get; set; }
    bool IsInteracting { get; }
    public INPC CurrentInteractionTarget { get; }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Sets the NPC's full name.
    /// </summary>
    public void SetFullName(string given, string family);

    /// <summary>
    /// Returns true if the NPC is available to start an interaction
    /// </summary>
    public bool IsAvailableForInteraction();

    /// <summary>
    /// Called on the target villager when an initiator requests interaction.
    /// Returns true if accepted and the target is now reserved for interaction.
    /// </summary>
    public bool TryAcceptInteraction(INPC initiator);

    /// <summary>
    /// Called on the initiator villager to start the interaction
    /// </summary>
    public void StartInteraction();

    /// <summary>
    /// Ends an ongoing interaction on this villager (called on both participants)
    /// </summary>
    public void EndInteraction();
    #endregion
}
