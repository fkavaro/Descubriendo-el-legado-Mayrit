
/// <summary>
/// Interface for objects with behaviour.
/// </summary>
public interface IBehaviourEntityGeneric<T> : IBehaviourEntity
where T : ABehaviourSystem
{
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
