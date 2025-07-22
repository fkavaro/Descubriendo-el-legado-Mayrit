using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public abstract class AUIState : AState<UIManager, FiniteStateMachine<UIManager>>
{
    public UIDocument _UI;

    protected AUIState(string name, FiniteStateMachine<UIManager> stateMachine)
    : base(name, stateMachine) { }
}
