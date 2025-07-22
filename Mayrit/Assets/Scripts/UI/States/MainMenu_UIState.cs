using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MainMenu_UIState : AUIState
{
    public MainMenu_UIState(FiniteStateMachine<UIManager> stateMachine)
    : base("MainMenuUI", stateMachine)
    {
    }

    public override void StartState()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }
}