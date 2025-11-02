using UnityEngine;

public class House : ABuilding
{
    public int _householdSize = 1;

    // When enabled, increase town population
    public void OnEnable()
    {
        TownManager.Instance.UpdatePopulation(_householdSize);
    }

    // When disabled, decrease town population
    public void OnDisable()
    {
        TownManager.Instance.UpdatePopulation(-_householdSize);
    }
}
