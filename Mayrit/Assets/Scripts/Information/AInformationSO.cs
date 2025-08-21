using UnityEngine;

public abstract class AInformationSO : ScriptableObject
{
    [Header("General Information")]
    [SerializeField] private string _header;
    [SerializeField] private string _subHeader;

    [TextArea(5, 10)]
    [SerializeField] private string _description;

    [Header("Image Information")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _imageCaption;

    // Public properties for read-only acces
    public string Header => _header;
    public string SubHeader => _subHeader;
    public string Description => _description;
    public Sprite Icon => _icon;
    public Sprite Image => _image;
    public string ImageCaption => _imageCaption;
}
