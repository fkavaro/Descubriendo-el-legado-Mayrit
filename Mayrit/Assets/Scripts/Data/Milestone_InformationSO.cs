using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Milestone_InformationSO", menuName = "Scriptable Objects/Milestone_InformationSO")]
public class Milestone_InformationSO : AInformationSO
{
    [Header("Environment Settings")]
    [SerializeField] private float _wantedTime;

    public float WantedTime => _wantedTime;
}
