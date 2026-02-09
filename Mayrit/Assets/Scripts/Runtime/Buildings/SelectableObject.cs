using UnityEngine;
using System;


// public class SelectableObject : MonoBehaviour
// {
//     #region PROPERTY HELPERS
//     public bool IsCharacter => _isCharacter;
//     public DataSO Data => _data;
//     public OrbitalCameraValues OrbitalCameraValues => _orbitalCameraValues;
//     #endregion

//     #region EDITOR PROPERTIES
//     [SerializeField] bool _isCharacter = false;
//     [SerializeField] DataSO _data;
//     [SerializeField] OrbitalCameraValues _orbitalCameraValues;
//     #endregion

//     #region LYFE CYCLE
//     void Awake()
//     {
//         // Set layer based on type
//         if (_isCharacter)
//         {
//             int layer = LayerMask.NameToLayer("PlayableCharacter");
//             if (layer != -1)
//                 gameObject.layer = layer;
//             else
//                 Debug.LogWarning("Layer 'PlayableCharacter' not found. Please add it in Project Settings.", this);
//         }
//         else
//         {
//             int layer = LayerMask.NameToLayer("SelectableObject");
//             if (layer != -1)
//                 gameObject.layer = layer;
//             else
//                 Debug.LogWarning("Layer 'SelectableObject' not found. Please add it in Project Settings.", this);

//         }
//     }
//     #endregion
// }
