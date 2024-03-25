using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class Character : MonoBehaviour
{
    public CharacterSO characterData;
    public CharacterGrowth characterGrowth;

    public SkillBase skill;
    public PassiveAbilityBase passiveAbility;


    //캐릭터 스탯 구조 설명: 기본스탯과 Calc 스탯 2종류로 나뉜다.
    //기본 스탯: CharacterSO의 기초 스탯 + 캐릭터 레벨에 의한 성장 스탯이 저장된다. 캐릭터 레벨이 변동될 때마다 값이 바뀐다.
    //Clac 스탯: 기본 스탯을 기반으로 특성, 클래스, 장비로 증가한 스탯을 계산한 1차 결과 스탯. 캐릭터 클래스/스탯/장비가 변동할 때마다 값이 바뀐다. 

    //순서대로 체력, 공격력, 방어력, 이동거리, 치확, 치피, 주는 피해 증가, 받는 피해 감소


    //=======기본 스탯=======// 
    public Constants.AttackType CharacterAttackType { get; private set; }
    public int Health { get; set; }

    public int TotalHealth { get { return Health; } }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public int Mov { get; set; }

    private float criticalRate = 0.1f; //치명타 확률. 기본값은 0.1배. (10% 확률)
    public float CriticalRate { get { return criticalRate; } private set { criticalRate = math.clamp(value, 0, 1f); } }  //치확은 0%~100%로만 설정되도록 범위 제한.
    public float CriticalDMG { get; private set; } = 1.2f;  //치명타 피해. 기본값은 1.2배.

    public float InflictDamageRatio { get; private set; } = 1f;//가하는 피해 비율. 기본값은 1배. 값이 0.1 증가 = 주는 피해 10% 증가.
    private float takenDamageRatio = 0f; // 받는 피해 비율. 기본값인 0일 때 100% 피해를 받는다. 값이 0.1 증가 = 받는 피해 10% 감소.
    public float TakenDamageRatio { get { return takenDamageRatio; } private set { criticalRate = math.clamp(value, 0, 1f); } } //받는 피해 감소는 0% ~ 100%로만 설정되도록 범위 제한. 0일 때 100%피해를 입고, 100일 때 0% 피해를 입음.

    //=======기본 스탯=======// 



    //Calc 스탯 : 1차적으로 계산된 스탯. 특성, 클래스, 장비 등 대체로 상시 적용되는 능력치 증감이 계산된 값을 저장한다.
    //=======Calc 스탯=======// 
    public int CalcHealth { get; set; }
    public int CalcAtk { get; set; }
    public int CalcDef { get; set; }
    public int CalcMov { get; set; }
    private float calcCrtRate;
    public float CalcCrtRate { get { return calcCrtRate; } set { calcCrtRate = math.clamp(value + calcCrtRate, 0, 1f); } }  //치확은 0%~100%로만 설정되도록 범위 제한.
    public float CalcCrtDMG { get; set; }
    public float CalcInflictDMGRatio { get; set; }
    private float calcTakenDMGRatio;
    public float CalcTakenDMGRatio { get { return calcTakenDMGRatio; } set { calcTakenDMGRatio = math.clamp(value, 0, 1f); } }
    //=======Calc 스탯=======// 




    //아마도 전투 시작시에 실행되는 Init 메서드
    public void CharacterInit() 
    {
        //characterGrowth.Init(this);
        //StatRefresh();

        Health = characterData.health;
        Attack = characterData.atk;
        Mov = characterData.mov;
        CharacterAttackType = characterData.attackType;
    }

    //스탯 최신화 메서드. 
    public void StatRefresh() 
    {
        Managers.CharacterGrowthManager.Init(this);
        Managers.CharacterGrowthManager.ApplyGrowStat();
        Managers.CharacterGrowthManager.ApplyAdditionStat();
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

}
