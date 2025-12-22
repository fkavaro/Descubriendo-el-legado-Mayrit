using UnityEngine;

[System.Serializable]
public class MilestoneMapping
{
    [SerializeField] DataSO _milestoneData;
    [SerializeField] GameObject _playableCharacter;
    [SerializeField] GameObject _tour;
    [SerializeField] float _wantedTime;
    [SerializeField] int _milestoneIndex;

    public DataSO Data => _milestoneData;
    public PlayableCharacter PlayableCharacter => _playableCharacter.GetComponent<PlayableCharacter>();
    public Tour Tour => _tour.GetComponent<Tour>();
    public float WantedTime => _wantedTime;
    public int Index
    {
        get => _milestoneIndex;
        set => _milestoneIndex = value;
    }
}
