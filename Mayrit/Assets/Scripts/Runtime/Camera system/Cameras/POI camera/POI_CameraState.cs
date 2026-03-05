using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class POI_CameraState : ACameraState
{
    public POI_CameraState(float simulationSpeed)
    : base("POI camera", null, simulationSpeed) { }
}
