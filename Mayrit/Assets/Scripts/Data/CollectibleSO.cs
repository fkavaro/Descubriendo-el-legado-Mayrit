using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleSO", menuName = "Scriptable Objects/Collectible")]
public class CollectibleSO : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] DataSO _data;
    [SerializeField] List<string> _hints;

    public int ID => _id;
    public DataSO Data => _data;
    public List<string> Hints => _hints;
}
