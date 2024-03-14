using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Playables;
using UnityEngine;
using static Constants;

public class Character : MonoBehaviour
{
    public CharacterSO characterData;

    public SkillBase skill;
    public PassiveAbilityBase passiveAbility;

    public int level = 1;
    public int maxLevel;
    public int exp;
    public int maxExp;

    //선택 중인 특성 번호. ( 0 = None )
    public int selectTrait_Tier1;
    public int selectTrait_Tier2;
    public int selectTrait_Tier3;

    //선택 중인 기본 클래스, 상위 클래스 번호 ( 0 = None )
    public int baseClass;
    public int superialClass;


    public Star star { get; private set; }


    //기본 스탯. 즉, Base 스탯. 캐릭터 레벨에 영향을 받음.
    public Constants.AttackType CharacterAttackType { get; private set; }
    public int Health { get; private set; }

    public int TotalHealth { get { return Health; } }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int Mov { get; private set; }

    private float criticalRate = 0.1f; //치명타 확률. 기본값은 0.1배. (10% 확률)
    public float CriticalRate { get { return criticalRate; } private set { criticalRate = math.clamp(value + criticalRate, 0, 1f); } }  //치확은 0%~100%로만 설정되도록 범위 제한.
    public float CriticalDMG { get; private set; } = 1.2f;  //치명타 피해. 기본값은 1.2배.

    public float InflictDamageRatio { get; private set; } = 1f;//가하는 피해 비율. 기본값은 1배.
    public float TakenDamageRatio { get; private set; } = 1f;//입는 피해 비율 기본값은 1배.


    //1차적으로 계산된 스탯. 특성, 클래스, 패시브 등 대체로 영구적으로 적용되는 능력치 증감이 계산된 값을 저장함.
    public int CalcHealth;
    public int CalcAtk;
    public int CalcDef;
    public int CalcMov;
    public float CalcCrtRate;
    public float CalcDMGRate;
    public float CalcInflictDMGRatio;
    public float CalcTakenDMGRatio;

    //기초 스탯 적용
    public void CharacterInit() //캐릭터 획득시 스테이터스 적용
    {
        CharacterAttackType = characterData.attackType;
        Health = characterData.health;
        Attack = characterData.atk;
        Defence = characterData.def;
        //Resistacne = characterData.res;
        Mov = characterData.mov;
        
        star = characterData.defaltStar; //캐릭터의 성급은 획득 시 기본 성급.
        maxLevel = (int)star; //최대 레벨은 기본 성급에 의해 정해짐.

        //selectTrait_Tier1 = characterData.trait_Tier1;
        ApplyGrowStat();
    }

    //성장 스탯 적용
    private void ApplyGrowStat()  //원본캐릭터의 기본 스탯 + (원본 캐릭터의 성장 스탯 * 이 캐릭터 개체의 현재 레벨)
    {
        Health = (characterData.health + (characterData.growHealth * level));
        Attack = (characterData.atk + (characterData.growAtk * level));
        Defence = (characterData.def + (characterData.growDef * level));
    }

    private void ApplyTraitStat() //계산식: (기초스탯 + 합연산 목록) * (곱연산 목록들의 합)
    {
        if(characterData.trait_Tier2[selectTrait_Tier2 - 1] != null)
        {

        }
        CalcHealth = (int)((Health + characterData.trait_Tier1.increaseHealth)*(characterData.trait_Tier1.multiplyHealth));
        CalcDef = (int)((Defence + characterData.trait_Tier1.multiplyDef) * (characterData.trait_Tier1.multiplyDef));
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

    //특성 적용
    //클래스 적용
    
}
