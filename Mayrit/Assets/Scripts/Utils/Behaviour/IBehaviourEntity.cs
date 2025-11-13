using UnityEngine;

public interface IBehaviourEntity
{
    string Name { get; }
    GameObject GO { get; }
    bool DebugMode { get; set; }
    bool IsExecutionPaused { get; set; }
    string CurrentActionInfo { get; set; }
}
