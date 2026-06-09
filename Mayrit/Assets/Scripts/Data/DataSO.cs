using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSO", menuName = "Scriptable Objects/Data")]
public class DataSO : ScriptableObject
{
    public enum DataType
    {
        Milestone,
        Player,
        POI,
        ModernBuilding,
        Other,
        Collectible
    }


    [Header("General information")]
    [SerializeField] DataType _dataType = DataType.Other;
    [SerializeField] string _header;
    [SerializeField] string _subHeader;
    [SerializeField] bool _showDisclaimer;
    [SerializeField] string _disclaimer = "Este modelo 3D no es representativo.";
    [SerializeField] List<GlosaryDefinitionSO> _glosaryDefinitions;


    [TextArea(5, 10)]
    [SerializeField] string _description;
    [TextArea(5, 10)]
    [SerializeField] string _conservation;

    [SerializeField] List<string> _sources;

    [Header("Image information")]
    [SerializeField] Sprite _image;
    [TextArea(3, 10)]
    [SerializeField] string _imageCaption;

    // PROPERTY HELPERS
    public DataType Type => _dataType;
    public bool IsMilestone => _dataType == DataType.Milestone;
    public bool IsPlayer => _dataType == DataType.Player;
    public bool IsPOI => _dataType == DataType.POI;
    public bool IsModernBuilding => _dataType == DataType.ModernBuilding;
    public bool IsCollectible => _dataType == DataType.Collectible;
    public bool IsOther => _dataType == DataType.Other;
    public string Header => _header;
    public string SubHeader => _subHeader;
    public bool ShowDisclaimer => _showDisclaimer;
    public string Disclaimer => _disclaimer;
    public List<GlosaryDefinitionSO> GlosaryDefinitions => _glosaryDefinitions;
    public string Description => _description;
    public string Conservation => _conservation;
    public List<string> Sources => _sources;
    public Sprite Image => _image;
    public string ImageCaption => _imageCaption;
}
