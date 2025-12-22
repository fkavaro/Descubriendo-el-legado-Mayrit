using UnityEngine;

public abstract class ATimedNPCStrategy<NPCtype> : ANPCStrategy<NPCtype>
where NPCtype : INPC
{
    readonly float _duration;
    float _elapsedTime;

    public ATimedNPCStrategy(NPCtype npc, float min = 30, float max = 120)
    : base(npc)
    {
        _duration = Random.Range(min, max);
        _elapsedTime = 0f;
    }

    public override Node.Status Update()
    {
        // Update elapsed time
        _elapsedTime += Time.deltaTime;

        // Check if duration is completed
        if (_elapsedTime >= _duration)
        {
            OnTimerComplete();
            _elapsedTime = 0f;

            return Node.Status.Success;
        }
        return Node.Status.Running;
    }

    public virtual void OnTimerComplete() { }
}
