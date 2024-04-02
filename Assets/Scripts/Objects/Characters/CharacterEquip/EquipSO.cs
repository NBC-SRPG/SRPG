using System.Collections.Generic;
using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "EquipData", fileName = "Equip_")]
public class EquipSO : ScriptableObject
{
    [Header("Equip_description")]
    public string equipName;//장비 이름
    public string equipDescription;//장비 설명
    

    [Header("Equip_Status_basic")] //increase = 깡스탯 증가량(합연산). multiply = 배율(%) 증가량(곱연산)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth; //곱연산은 계산할 때 1을 더하고 시작하므로, 이 특성으로 10%를 증가시키고 싶다면 0.1로 설정하는 방식으로 설정하면 됩니다.
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Equip_Status_addition")]
    public float increasecCtr; //치명타 확률 +
    public float increasecCtd; //치명타 피해 +
    public float increaseInflictDamage; //주는 최종 피해 증가 +
    public float reducedTakenDamage; //받는 최종 피해 감소 -


    [Header("Equip_special")]
    public int speciaID;//장비의 별도 고유 효과. 세부로직 미구현.

    [Header("Develope")]
    public int equip_Id;//장비 식별자
    public EquipType equipType; //장비 타입
    public ItemRank ItemRank;
    public Dictionary<int, int> rankUpMaterials; //돌파하는데 필요한 소재 아이템의 ID값 / 필요한 갯수 딕셔너리
}
