using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterSO characterData;

    public SkillBase skill;
    public PassiveAbilityBase passiveAbility;

    public int skillLevel;
    public int passiveLevel;

    public int level;
    public int maxLevel;
    public int exp;
    public int maxExp;


    public Constants.AttackType CharacterAttackType { get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int Mov { get; private set; }

    //기초 스탯 적용
    public void CharacterInit()
    {
        CharacterAttackType = characterData.attackType;
        Health = characterData.health;
        Attack = characterData.atk;
        Defence = characterData.def;
        //Resistacne = characterData.res;
        Mov = characterData.mov;

        ApplyGrowStat();
    }

    //성장 스탯 적용
    private void ApplyGrowStat()  //원본 캐릭터의 기본 스탯 + (원본 캐릭터의 성장 스탯 * 이 캐릭터 개체의 현재 레벨)
    {
        Health = (characterData.health + (characterData.growHealth * level));
        Attack = (characterData.atk + (characterData.growAtk * level));
        Defence = (characterData.def + (characterData.growDef * level));
    }

    //스킬 선언
    public SkillBase InitSkills()
    {
        skill = new SkillBase(characterData.skill);

        return skill;
    }

    //패시브 선언
    public PassiveAbilityBase InitPassive()
    {
        Type passiveType = Type.GetType("PassiveAbility_" + characterData.passive.passive_Id);

        object obj = Activator.CreateInstance(passiveType);
        passiveAbility = obj as PassiveAbilityBase;

        return passiveAbility;
    }

    //레벨업


    //돌파
}
