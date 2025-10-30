using UnityEngine;
using UnityEngine.AI;

public class Villager : ANPC<FiniteStateMachine>
{
    public override FiniteStateMachine BehaviourSystem => throw new System.NotImplementedException();

    public override void InitializeBehaviour()
    {

    }
}
