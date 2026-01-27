using System;
using UnityEngine.AI;
using UnityEngine;

public interface INPC : ICharacter
{
    public enum RoleInConversation
    {
        Initiator,
        Follower,
        None
    }

    #region PROPERTIES
    public NavMeshAgent Agent { get; }
    NPCMovementController MovementController { get; }
    NPCInteractionController InteractionController { get; }
    float AvoidanceRadius { get; }
    float MaxSamplingDistance { get; }
    int AvoidancePriorityVariance { get; }
    int BaseAvoidancePriority { get; }
    public float WalkSpeedVariance { get; }
    RoleInConversation ConversationRole { get; set; }
    bool InAccessZone { get; set; }
    bool HasArrivedToMiddlePoint { get; set; }
    public INPC CurrentConversationTarget { get; set; }
    public GameObject CurrentConversationTargetGO { get; set; }
    public INPC LastConversationTarget { get; set; }
    public GameObject LastConversationTargetGO { get; set; }
    public float ConversationDuration { get; set; }
    public House Home { get; }
    public Workplace Workplace { get; }
    public Sanctuary Sanctuary { get; }
    public Market Market { get; }
    public Stall MarketStall { get; set; }
    public bool IsWaitingForAccess { get; set; }
    #endregion

    #region METHODS
    public void SetCharacterAndAgentActive(bool isActive);
    #endregion
}
