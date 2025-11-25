using UnityEngine;

[System.Serializable]
public class MilestoneMapping
{
    [SerializeField] Milestone_InformationSO _milestoneData;
    [SerializeField] GameObject _playableCharacter;
    [SerializeField] GameObject _tour;

    public Milestone_InformationSO Data => _milestoneData;
    public PlayableCharacter PlayableCharacter => _playableCharacter.GetComponent<PlayableCharacter>();
    public Tour Tour => _tour.GetComponent<Tour>();
}
