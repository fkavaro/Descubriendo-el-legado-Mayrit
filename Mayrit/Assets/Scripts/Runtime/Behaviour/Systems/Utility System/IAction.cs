
public interface IAction
{
    public string ActionName { get; }
    public float Utility { get; }
    public abstract void StartAction();
    public abstract void UpdateAction();
    public virtual void FinishAction() { }
    public abstract bool IsFinished();
    public abstract string DebugAction();
    public virtual void Reset() { }
}
