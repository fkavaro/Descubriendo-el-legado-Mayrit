using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MilestonesTimesSO", menuName = "Scriptable Objects/MilestonesTimes")]
public class MilestonesTimesSO : ScriptableObject
{
    public List<MilestoneTime> List => _list;
    [SerializeField] List<MilestoneTime> _list = new();
}

[Serializable]
public class MilestoneTime
{
    public SceneDatabase.SceneName _type;
    public float _time;
}
