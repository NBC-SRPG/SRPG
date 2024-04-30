using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EquipData", fileName = "EquipSO_")]
public class EquipSO : PassiveSO
{
    [Header("Develope")]
    public int id;

    [Header("Equip_description")]
    public string equipName;
    [TextArea]
    public string description;
    public int star;

    public Sprite sprite;
    
    [Header("VisibleStatus")]
    public int hp;
    public int atk;
    public int def;
    public int mov;

    [Header("InvisibleStatus")]

    public string additionalOption;   // 추가효과를 UI에 나타내기 위한 설명
    public int atkIncrease;     // 공격력 %증가량 (곱연산)
    public int defIncrease;     // 방어력 %증가량 (곱연산)

    public int critRate;       // 치명타 확률 (합연산)
    public int critDmg;         // 치명타 데미지 (합연산)

    public int EnhancedDmg;     // 데미지 증가 (합연산)
    public int ReducedDmg;     // 받는 데미지 감소 (곱연산)


    [Header("Upgrade")]
    public int upgradeLevel;
    public Dictionary<int, int> upgradeMaterials = new();
    public int gold;
}
