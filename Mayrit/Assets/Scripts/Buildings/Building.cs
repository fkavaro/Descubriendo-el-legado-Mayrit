using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] List<Spot> _entranceSpots;

    public void PlaceAtRandomEntrance(INPC npc)
    {
        Spot entranceSpot = GetRandomEntranceSpot();
        if (entranceSpot != null)
        {
            npc.Agent.transform.position = entranceSpot.transform.position;
            npc.ForceRotation(entranceSpot.DirectionWorldQuaternion);
        }
        else
        {
            npc.Agent.transform.position = transform.position;
            Debug.LogWarning($"No entrance spots defined for building {gameObject.name}. Placing villager at building position.");
        }
    }

    public Spot GetRandomEntranceSpot()
    {
        if (_entranceSpots.Count == 0) return null;
        int randomIndex = Random.Range(0, _entranceSpots.Count);
        return _entranceSpots[randomIndex];
    }
}
