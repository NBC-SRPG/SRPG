using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class Character : MonoBehaviour
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
    public ExSkillBase exSkill;
    public PassiveSkillBase passiveSkill;


    public AbilitySO abilityT1;
    public AbilitySO abilityT2;
    public AbilitySO abilityT3;

    public ClassSO basicClass;
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

        // EX스킬 초기화
        Utility.Id2SO<SkillSO>(SO.id, (result) =>
        {
            exSkill = new ExSkillBase((SkillSO)result);
        });

        // TODO: passiveSkill 초기화


        // 특성 초기화
        Utility.Id2SO<AbilitySO>(SO.abilityT1, (result) =>
        {
            abilityT1 = (AbilitySO)result;
        });

        Utility.Id2SO<AbilitySO>(SO.abilityT2[Growth.abilityT2], (result) =>
        {
            abilityT2 = (AbilitySO)result;
        });

        Utility.Id2SO<AbilitySO>(SO.abilityT3[Growth.abilityT3], (result) =>
        {
            abilityT3 = (AbilitySO)result;
        });

        // 클래스 초기화
        Utility.Id2SO<ClassSO>(SO.basicClass, (result) =>
        {
            basicClass = (ClassSO)result;
        });

        Utility.Id2SO<ClassSO>(SO.superiorClass[Growth.superiorClass], (result) =>
        {
            superiorClass = (ClassSO)result;
        });
    }

    private void CalculateStat()
    {
        hp = SO.hp + SO.hpPerLv * Growth.level;
        atk = SO.atk + SO.atkPerLv * Growth.level;
        def = SO.def + SO.defPerLv * Growth.level;
    }




    /*
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
    */

}
