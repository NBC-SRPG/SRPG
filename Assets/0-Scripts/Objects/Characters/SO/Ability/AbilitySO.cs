using UnityEngine;


[CreateAssetMenu(menuName = "AbilityData", fileName = "AbilitySO_")]
public class AbilitySO : PassiveSO
{
    [Header("Develope")]
    public int id;

    [Header("Description")]
    public string abilityName;      //특성 이름
    public string description;       //특성 설명
    public Sprite icon;

}
