using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public Constants.AttackType attackType;//공격 타입
    public Constants.CharacterAttribute characterAttribute;//캐릭터 속성

    [Header("Develope")]
    public string character_Id;//캐릭터 식별자

    [Header("Status")]//캐릭터 능력치
    public int health;
    public int atk;
    public int def;
    public int mov;
    [Range(0, 10)]public int atk_range;//공격 사정거리(근거리의 경우 0으로)

    [Header("GrowthStatus")]//캐릭터 성장 능력치. 1레벨 당 해당 value 값만큼 증가.
    public int growHealth;
    public int growAtk;
    public int growDef;

    [Header("Story")]
    public string characterName;//캐릭터 이름
    public string story;//캐릭터 스토리
    public Constants.Belonging belonging;//캐릭터 소속

    [Header("SkillList")]
    public SkillSO skill;
    public PassiveSO passive;

    [Header("Star")]
    public int defaltStar; //기본 성급
}
