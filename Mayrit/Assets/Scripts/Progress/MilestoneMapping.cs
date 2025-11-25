using UnityEngine;

[System.Serializable]
public class MilestoneMapping
{
    [SerializeField] Milestone_InformationSO _milestoneData;
    [SerializeField] GameObject _playableCharacterGO;
    [SerializeField] GameObject _tour;

    public Milestone_InformationSO Data => _milestoneData;
    public PlayableCharacter PlayableCharacter => _playableCharacterGO.GetComponent<PlayableCharacter>();
    public Tour Tour => _tour.GetComponent<Tour>();
}
