using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TraitType
{
    Stat, //단순히 스탯 (체력/방어력/공격력, MOV, 치확, 치피, 뎀증, 받피감) 수치만 상승시켜줍니다.
    Special //스탯 외에 부가적인 특수 메커니즘을 추가하거나 변경 시킵니다. 
}

[CreateAssetMenu(menuName = "TraitData", fileName = "Trait_")]
public class TraitSO : ScriptableObject
{
    [Header("Develope")]
    public string trait_Id;//캐릭터 식별자

    [Header("Trait_description")]
    public string traitName;//특성 이름
    public string traitDescription;//특성 설명

    [Header("Image")]
    public Sprite traitSprite;//특성 스프라이트(=썸네일or아이콘)

    [Header("Trait_Status")] //increase = 깡스탯 증가량(합연산). multiply = 배율(%) 증가량(곱연산)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public int multiplyHealth;
    public int multiplyAtk;
    public int multiplyDef;
    public int multiplyMov;

    [Header("Trait_special")]
    public string speciaID;//특수

}
