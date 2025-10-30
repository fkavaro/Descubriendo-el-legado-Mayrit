using UnityEngine;

/// <summary>
/// Interface for objects with behaviour.
/// </summary>
public interface IBehaviourEntity<T>
where T : ABehaviourSystem
{
    bool DebugMode { get; set; }
    bool IsExecutionPaused { get; set; }

    /// <summary>
    /// The behaviour system should be initialized here.
    /// Is executed in Monobehaviour/Awake().
    /// </summary>
    void InitializeBehaviour();

    /// <summary>
    /// The behaviour system of the object.
    /// </summary>
    T BehaviourSystem { get; }
}
