using System;
using UnityEngine;

public abstract class ATutorialStepConditionSO : ScriptableObject
{
    public event Action Completed;

    public virtual void BeginListening() { }
    public virtual void EndListening() { }
    public virtual void Tick(float deltaTime) { }

    protected void MarkCompleted() => Completed?.Invoke();
}
