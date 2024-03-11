using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterSO characterData;

    public List<SkillBase> skillList;
    public List<PassiveAbilityBase> passiveAbilityList;

    public int level;
    public int exp;

    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int Resistacne { get; private set; }
    public int Agility { get; private set; }

    //기초 스탯 적용
    public void CharacterInit()
    {
        Health = characterData.health;
        Attack = characterData.atk;
        Defence = characterData.def;
        Resistacne = characterData.res;
        Agility = characterData.agi;

        ApplyGrowStat();
    }

    //성장 스탯 적용
    private void ApplyGrowStat()
    {
        
    }

    public void InitSkills()
    {
        skillList = new List<SkillBase>();

        foreach(SkillSO skills in characterData.skills)
        {
            SkillBase skill = new SkillBase(skills);

            skillList.Add(skill);
        }
    }

    public void InitPassive()
    {
        passiveAbilityList = new List<PassiveAbilityBase>();

        foreach (PassiveSO passive in characterData.passives)
        {
            PassiveAbilityBase passiveAbility;

            Type passiveType = Type.GetType("PassiveAbillity_" + passive.passive_Id);

            object obj = Activator.CreateInstance(passiveType);
            passiveAbility = obj as PassiveAbilityBase;

            passiveAbilityList.Add(passiveAbility);
        }
    }
}
