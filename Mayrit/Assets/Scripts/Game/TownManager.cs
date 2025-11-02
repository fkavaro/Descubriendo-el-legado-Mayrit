using System;
using UnityEngine;

public class TownManager : Singleton<TownManager>
{
    [Header("Town Stats")]
    public int _population;

    public void UpdatePopulation(int householdSize)
    {
        _population += householdSize;
    }
}
