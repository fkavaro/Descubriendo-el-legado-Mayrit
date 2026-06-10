using UnityEngine;

[CreateAssetMenu(fileName = "GlosaryDefinitionSO", menuName = "Scriptable Objects/Glosary definition")]
public class GlosaryDefinitionSO : ScriptableObject
{
    [SerializeField] string _word;
    [TextArea(5, 5)]
    [SerializeField] string _definition;

    public string Word => _word;
    public string Definition => _definition;
    public string WordWithDefinition => $"<b>{_word}</b>: {_definition}";
}
