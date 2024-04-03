using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class Character
{
    public CharacterSO SO;
    public CharacterGrowth Growth;
    

    [Header("VisibleStatus")]
    public int hp;
    public int atk;
    public int def;
    public int mov;


    [Header("InvisibleStatus")]
    public int atkIncrease;     // 공격력 %증가량 (곱연산)
    public int defIncrease;     // 방어력 %증가량 (곱연산)

    public int critRate;       // 치명타 확률 (합연산)
    public int critDmg;         // 치명타 데미지 (합연산)

    public int EnhancedDmg;     // 데미지 증가 (합연산)
    public int ReducedDmg;     // 받는 데미지 감소 (곱연산)


    [Header("Skill")]
    // TODO: refactor with chai227chai
    public SkillBase skill;
    public PassiveAbilityBase passiveAbility;


    public AbilitySO abilityT1;
    public AbilitySO abilityT2;
    public AbilitySO abilityT3;

    public ClassSO superiorClass;


    public Character(CharacterSO SO, CharacterGrowth Growth)
    {
        this.SO = SO;
        this.Growth = Growth;

        CalculateStat();
        mov = SO.mov;

        atkIncrease = 0;
        defIncrease = 0;

        critRate = 20;      // 기본 치명타 확률 20%
        critDmg = 50;       // 기본 치명타 데미지 50%

        EnhancedDmg = 0;
        ReducedDmg = 0;

        // TODO: Skills
        //skill = null;
        //passiveAbility = null;

        //abilityT1 = null;
        //abilityT2 = null;
        //abilityT3 = null;

        //superiorClass = null;
    }

    private void CalculateStat()
    {
        hp = SO.hp + SO.hpPerLv * Growth.level;
        atk = SO.atk + SO.atkPerLv * Growth.level;
        def = SO.def + SO.defPerLv * Growth.level;
    }




    public SkillBase InitSkills()
    {
        skill = new SkillBase(5);

        return skill;
    }


    public PassiveAbilityBase InitPassive()
    {
        Type passiveType = Type.GetType("PassiveAbility_" + SO.PassiveSkill);

        if(passiveType == null)
        {
            return null;
        }

        object obj = Activator.CreateInstance(passiveType);
        passiveAbility = obj as PassiveAbilityBase;

        return passiveAbility;
    }

}
