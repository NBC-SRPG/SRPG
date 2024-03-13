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

    public int level;
    public int exp;

    public Constants.AttackType CharacterAttackType { get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int Resistacne { get; private set; }
    public int Mov { get; private set; }

    //���� ���� ����
    public void CharacterInit()
    {
        CharacterAttackType = characterData.attackType;
        Health = characterData.health;
        Attack = characterData.atk;
        Defence = characterData.def;
        Resistacne = characterData.res;
        Mov = characterData.mov;

        ApplyGrowStat();
    }

    //���� ���� ����
    private void ApplyGrowStat()
    {
        
    }

    //��ų ����
    public SkillBase InitSkills()
    {
        skill = new SkillBase(characterData.skill);

        return skill;
    }

    //�нú� ����
    public PassiveAbilityBase InitPassive()
    {
        Type passiveType = Type.GetType("PassiveAbility_" + characterData.passive.passive_Id);

        object obj = Activator.CreateInstance(passiveType);
        passiveAbility = obj as PassiveAbilityBase;

        return passiveAbility;
    }
}
