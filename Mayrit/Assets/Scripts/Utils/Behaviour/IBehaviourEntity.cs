using UnityEngine;

public interface IBehaviourEntity
{
    GameObject GO { get; }
    bool DebugMode { get; set; }
    bool IsExecutionPaused { get; set; }
}
