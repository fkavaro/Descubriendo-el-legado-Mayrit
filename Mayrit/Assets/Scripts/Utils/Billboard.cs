// https://youtu.be/FjJJ_I9zqJo?si=COnrovYRfn-TG86U 
using UnityEngine;


public class Billboard : MonoBehaviour
{
    [Tooltip("Wether to fully follow camera or just in Y axis")]
    [SerializeField] bool freezeXZAxis = false;

    void Update()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}