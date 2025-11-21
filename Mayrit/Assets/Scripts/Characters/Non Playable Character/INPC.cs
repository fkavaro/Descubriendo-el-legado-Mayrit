using UnityEngine;
using UnityEngine.AI;

public interface INPC : ICharacter
{
    #region PROPERTIES HELPERS
    public NavMeshAgent Agent { get; }
    NPCMovementController MovementController { get; }
    float AvoidanceRadius { get; }
    float MaxSamplingDistance { get; }
    int AvoidancePriorityVariance { get; }
    int BaseAvoidancePriority { get; }
    bool IsStopped { get; set; }
    public string GivenName { get; }
    public string FamilyName { get; }
    bool IsInStreet { get; set; }
    bool IsTalking { get; set; }
    bool IsBeingTalkedTo { get; set; }
    bool IsReadyToTalk { get; set; }
    public INPC CurrentInteractionTarget { get; set; }
    public INPC LastInteractionTarget { get; set; }
    #endregion

    #region METHODS
    /// <summary>
    /// Sets the NPC's full name.
    /// </summary>
    public void SetFullName(string given, string family);

    /// <summary>
    /// Returns true if is in the street and its model is active.
    /// </summary>
    /// <returns></returns>
    public bool IsAvailableForConversation();

    /// <summary>
    /// Returns true if the character is available to start an interaction.
    /// </summary>
    public bool CanAcceptConversation(INPC initiator);

    /// <summary>
    /// Called on the initiator character to start the interaction
    /// </summary>
    public void StartConversation();

    /// <summary>
    /// Ends an ongoing interaction on this character (called on both participants)
    /// </summary>
    public void EndConversation();
    #endregion
}
