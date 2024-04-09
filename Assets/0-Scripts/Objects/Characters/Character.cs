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
    public ExSkillBase exSkill;
    public PassiveLogic passiveSkill;
    public PassiveSkillSO passive;

    public AbilitySO abilityT1;
    public AbilitySO abilityT2;
    public AbilitySO abilityT3;

    public ClassSO basicClass;
    public ClassSO superiorClass;

    public EquipSO weapon;
    public EquipSO armor;


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
        Utility.Id2SO<ExSkillSO>(SO.id, (result) =>
        {
            exSkill = new ExSkillBase((ExSkillSO)result);
        });

        // TODO: passiveSkill 초기화
        Utility.Id2SO<PassiveSkillSO>(SO.id, (result) =>
        {
            passive = (PassiveSkillSO)result;
            passiveSkill = Utility.GetAbilityBySO(passive);
        });

        // 특성 초기화
        Utility.Id2SO<AbilitySO>(SO.abilityT1, (result) =>
        {
            abilityT1 = (AbilitySO)result;
        });

        if (Growth.abilityT2 !=-1)
        {
            Utility.Id2SO<AbilitySO>(SO.abilityT2[Growth.abilityT2], (result) =>
        {
            abilityT2 = (AbilitySO)result;
        });

        }

        if (Growth.abilityT3 !=-1)
        {
            Utility.Id2SO<AbilitySO>(SO.abilityT3[Growth.abilityT3], (result) =>
        {
            abilityT3 = (AbilitySO)result;
        });
        }


        // 클래스 초기화
        Utility.Id2SO<ClassSO>(SO.basicClass, (result) =>
        {
            basicClass = (ClassSO)result;
        });

        if (Growth.superiorClass !=-1)
        {
            Utility.Id2SO<ClassSO>(SO.superiorClass[Growth.superiorClass], (result) =>
        {
            superiorClass = (ClassSO)result;
        });
        }

        // 장비 초기화
        Utility.Id2SO<EquipSO>(SO.weapon[Growth.weapon], (result) =>
        {
            weapon = (EquipSO)result;
        });

        Utility.Id2SO<EquipSO>(SO.armor[Growth.armor], (result) =>
        {
            armor = (EquipSO)result;
        });
    }

    private void CalculateStat()
    {
        hp = SO.hp + SO.hpPerLv * Growth.level;
        atk = SO.atk + SO.atkPerLv * Growth.level;
        def = SO.def + SO.defPerLv * Growth.level;
    }

}
