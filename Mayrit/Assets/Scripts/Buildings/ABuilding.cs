using System.Collections.Generic;
using UnityEngine;

public abstract class ABuilding : MonoBehaviour
{
    public List<Spot> entranceSpots;

    public Spot GetRandomEntranceSpot()
    {
        if (entranceSpots.Count == 0) return null;
        int randomIndex = Random.Range(0, entranceSpots.Count);
        return entranceSpots[randomIndex];
    }
}
