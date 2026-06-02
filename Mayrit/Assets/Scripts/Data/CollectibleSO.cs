using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleSO", menuName = "Scriptable Objects/Collectible")]
public class CollectibleSO : ScriptableObject
{
    [SerializeField] DataSO _data;
    [SerializeField] List<string> _hints;

    public DataSO Data => _data;
    public List<string> Hints => _hints;
}
