using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Range,
    Mage
}

public enum CharacterAttribute
{
    Fire,
    Water,
    Grass,
    Bolt,
    Dark,
    Light
}

public enum Belonging
{
    Demon,
    League,
    Order,
    Vegabond
}

[CreateAssetMenu(menuName = "CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public AttackType attackType;//공격 타입
    public CharacterAttribute characterAttribute;//캐릭터 속성

    [Header("Develope")]
    public string character_Id;//캐릭터 식별자

    [Header("Status")]//캐릭터 능력치
    public int health;
    public int atk;
    public int def;
    public int res;
    public int mov;
    [Range(0, 10)]public int atk_range;//공격 사정거리(근거리의 경우 0으로)

    [Header("Story")]
    public string characterName;//캐릭터 이름
    public string story;//캐릭터 스토리
    public Belonging belonging;//캐릭터 소속

    [Header("SkillList")]
    public SkillSO skill;
    public PassiveSO passive;
}
