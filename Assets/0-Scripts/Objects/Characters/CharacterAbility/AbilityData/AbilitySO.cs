using UnityEngine;


[CreateAssetMenu(menuName = "AbilityData", fileName = "Ability_")]
public class AbilitySO : ScriptableObject
{
    [Header("Develope")]
    public int id;      //특성 식별자

    [Header("Ability_description")]
    public string abilityName;      //특성 이름
    public string abilityDescription;       //특성 설명


    public void abilityEffect()
    {
        // TODO: 이곳에 특성 효과 구현
    }

}
