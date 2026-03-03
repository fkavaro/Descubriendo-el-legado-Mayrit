using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "TimerConditionSO", menuName = "Scriptable Objects/Tutorial Conditions/Timer")]
public class TimerConditionSO : ATutorialStepConditionSO
{
    [SerializeField, Min(0f)] float _seconds = 2f;

    float _elapsed;
    bool _running;

    public override void BeginListening()
    {
        _elapsed = 0f;
        _running = true;
    }

    public override void EndListening()
    {
        _running = false;
        _elapsed = 0f;
    }

    public override void Tick(float deltaTime)
    {
        if (!_running) return;

        _elapsed += deltaTime;
        if (_elapsed >= _seconds)
        {
            EndListening();
            MarkCompleted();
        }
    }
}