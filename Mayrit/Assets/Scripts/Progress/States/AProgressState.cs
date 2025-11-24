using System;
using UnityEngine;

public abstract class AProgressState : AState
{
    public readonly Milestone_InformationSO _milestoneInformation;

    public AProgressState(string name,
        Milestone_InformationSO milestoneInformation)
    : base(name)
    {
        _milestoneInformation = milestoneInformation;
    }
}