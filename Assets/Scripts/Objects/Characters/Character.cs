using System;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class Character : MonoBehaviour
{
    public CharacterSO characterData;

    public SkillBase skill;
    public PassiveAbilityBase passiveAbility;

    public float ExSkillLevel { get; private set; } = 1; //Ex스킬 레벨 //Todo: 스킬 레벨업 메서드 만들기. 스킬 레벨에 따라 스킬 계수 적용시키기. 실제 인게임에서 스킬 레벨에 따라 효과 달라지게 하기.
    public float PassiveSkillLevel { get; private set; } = 1; //패시브 스킬 레벨 //계수가 있는 패시브 스킬은 계수를 올리면 되는데, 계수가 없는 패시브 스킬은 어떻게 할지, 패시브 스킬 레벨에 관한 의논 필요.

    private int level = 1;
    public int Level
    {
        get { return level;} 
        set { 
            level = math.clamp(value, 1, maxLevel);
            ApplyGrowStat();
            ApplyTraitClassStat(); //레벨업 시 성장 스탯 및 특성/클래스 스탯 리프레시.
            maxExp = (level * 100 + maxLevel*level*20); //임의로 최대 경험치값(레벨업하는데 필요한 경험치 값)을 설정. 
            }
    }
    public int maxLevel { get; private set; }
    private int exp;
    public int Exp
    {
        get { return exp; } 
        set //경험치 설정자에 자체적으로 레벨업 기능을 배치했습니다.
        { 
            exp = value; 
            while ( exp >= maxExp && level != maxLevel) 
            {
                exp -= maxExp;
                level += 1;
            }
            exp = math.clamp(exp, 0, maxExp);
        }
    }

    public int maxExp { get; private set; }

    //선택 중인 특성 번호. ( 0 = None )
    //이 변수들을 설정해 배열의 index 번호를 지정한다. 2, 3티어 특성 및 상위 클래스는 CharacterSO의 필드에 해당 SO의 배열 형식으로 저장되어있음.
    public int selectTrait_Tier1 { get; private set; }
    public int selectTrait_Tier2 { get; private set; }
    public int selectTrait_Tier3 { get; private set; }

    //선택 중인 기본 클래스, 상위 클래스 번호 ( 0 = None )
    public int baseClass { get; private set; }
    public int selectSuperialClass { get; private set; }


    public Star star { get; private set; }


    //기본 스탯.
    public Constants.AttackType CharacterAttackType { get; private set; }
    public int Health { get; private set; }

    public int TotalHealth { get { return Health; } }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    public int Mov { get; private set; }


    //추가 스탯. 치확, 치피, 주는 피해 증가, 받는 피해 감소. 실제 인게임 전투에서 적용되도록 작업 필요.
    private float criticalRate = 0.1f; //치명타 확률. 기본값은 0.1배. (10% 확률)
    public float CriticalRate { get { return criticalRate; } private set { criticalRate = math.clamp(value, 0, 1f); } }  //치확은 0%~100%로만 설정되도록 범위 제한.
    public float CriticalDMG { get; private set; } = 1.2f;  //치명타 피해. 기본값은 1.2배.

    public float InflictDamageRatio { get; private set; } = 1f;//가하는 피해 비율. 기본값은 1배. 값이 0.1 증가 = 주는 피해 10% 증가.
    private float takenDamageRatio = 0f; // 받는 피해 비율. 기본값인 0일 때 100% 피해를 받는다. 값이 0.1 증가 = 받는 피해 10% 감소.
    public float TakenDamageRatio { get { return takenDamageRatio; } private set { criticalRate = math.clamp(value, 0, 1f); } } //받는 피해 감소는 0% ~ 100%로만 설정되도록 범위 제한. 0일 때 100%피해를 입고, 100일 때 0% 피해를 입음.


    //Calc 스탯 : 1차적으로 계산된 스탯. 특성, 클래스, 패시브 등 대체로 상시 적용되는 능력치 증감이 계산된 값을 저장함.
    public int CalcHealth { get; private set; }
    public int CalcAtk { get; private set; }
    public int CalcDef { get; private set; }
    public int CalcMov { get; private set; }
    private float calcCrtRate;
    public float CalcCrtRate { get { return calcCrtRate; } private set { calcCrtRate = math.clamp(value + calcCrtRate, 0, 1f); } }  //치확은 0%~100%로만 설정되도록 범위 제한.
    public float CalcCrtDMG { get; private set; }
    public float CalcInflictDMGRatio { get; private set; }
    private float calcTakenDMGRatio;
    public float CalcTakenDMGRatio { get { return calcTakenDMGRatio; } private set { calcTakenDMGRatio = math.clamp(value, 0, 1f); } }


    //기초 스탯 적용
    public void CharacterInit() //캐릭터 최초 획득시 기초 스탯을 적용시키는 메서드.
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
        ApplyTraitClassStat();
    }

    //성장 스탯 적용
    private void ApplyGrowStat() //캐릭터 성장 스탯(레벨) 적용 메서드. 레벨값이 바뀔 때마다 호출됨
    {
        //캐릭터의 스탯 값을 (기본 스탯값 + 레벨에 의해 증가한 수치값)으로 저장한다.
        //계산식 : 스탯값 = 원본캐릭터의 기본 스탯 + (원본 캐릭터의 성장 스탯 * 이 캐릭터 개체의 현재 레벨)
        Health = (characterData.health + (characterData.growHealth * level));
        Attack = (characterData.atk + (characterData.growAtk * level));
        Defence = (characterData.def + (characterData.growDef * level));
    }

    private void ApplyTraitClassStat() //캐릭터 특성 / 클래스 스탯 적용 메서드
    {
        //특성으로 올라가는 능력치들의 합계를 저장할 변수를 선언한다.
        int traitIncrHp_sum = 0; //체력 상수 증가치 합계
        int traitIncrDef_sum = 0; //방어력
        int traitIncrAtk_sum = 0; //공격력
        int traitIncrMov_sum = 0; //이동거리

        float traitIncrCtr_sum = 0; //치명타 확률 ( 수치 0.1 = 치명타 확률 10%증가)
        float traitIncrCtd_sum = 0; //치명타 피해
        float traitIncrInfD_sum = 0; //주는 데미지 증가
        float traitIncrTakenD_sum = 0; //받는 피해 감소

        float traitMultiplHp_sum = 0; //체력 배율 증가치 합계
        float traitMultiplDef_sum = 0; //방어력 배율 증가치 합계
        float traitMultiplAtk_sum = 0; //공격력 배율 증가치 합계

        if (level >= 30) //레벨이 30 이상이어서 1차 특성이 해금되었는지 확인한다.
        {
            //티어1 특성은 하나 뿐이므로, 캐릭터SO가 가진 1티어 특성의 증가값을 가져와 합계에 저장한다.
            traitIncrHp_sum = characterData.trait_Tier1.increaseHealth;
            traitIncrDef_sum = characterData.trait_Tier1.increaseDef;
            traitIncrAtk_sum = characterData.trait_Tier1.increaseAtk;
            traitIncrMov_sum = characterData.trait_Tier1.increasecMov;

            traitIncrCtr_sum = characterData.trait_Tier1.increasecCtr;
            traitIncrCtd_sum = characterData.trait_Tier1.increasecCtd;
            traitIncrInfD_sum = characterData.trait_Tier1.increaseInflictDamage;
            traitIncrTakenD_sum = characterData.trait_Tier1.reducedTakenDamage;

            traitMultiplHp_sum = characterData.trait_Tier1.multiplyHealth;
            traitMultiplDef_sum = characterData.trait_Tier1.multiplyDef;
            traitMultiplAtk_sum = characterData.trait_Tier1.multiplyAtk;

            if (selectTrait_Tier2 != 0) //'선택한 2티어 특성'값이 0이라면 특성이 개방되지 않았거나 아직 선택하지 않은 경우.
            {
                //selectTrait_Tier 값이 0이 아니라면 특성을 선택했다는 뜻. 캐릭터SO가 가진 '선택가능한 티어 2 특성 배열'에서 일치하는 특성을 가져와 그 증가값을 합계에 더한다. 
                traitIncrHp_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increaseHealth;
                traitIncrDef_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increaseDef;
                traitIncrAtk_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increaseAtk;
                traitIncrMov_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increasecMov;

                traitIncrCtr_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increasecCtr;
                traitIncrCtd_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increasecCtd;
                traitIncrInfD_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].increaseInflictDamage;
                traitIncrTakenD_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].reducedTakenDamage;

                traitMultiplHp_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].multiplyHealth;
                traitMultiplDef_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].multiplyDef;
                traitMultiplAtk_sum += characterData.trait_Tier2[selectTrait_Tier2 - 1].multiplyAtk;
                if (selectTrait_Tier3 != 0)
                {
                    //3티어 특성도 마찬가지로 적용된다.
                    traitIncrHp_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increaseHealth;
                    traitIncrDef_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increaseDef;
                    traitIncrAtk_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increaseAtk;
                    traitIncrMov_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increasecMov;

                    traitIncrCtr_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increasecCtr;
                    traitIncrCtd_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increasecCtd;
                    traitIncrInfD_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].increaseInflictDamage;
                    traitIncrTakenD_sum += characterData.trait_Tier3[selectTrait_Tier3 - 1].reducedTakenDamage;

                    traitMultiplHp_sum += characterData.trait_Tier2[selectTrait_Tier3 - 1].multiplyHealth;
                    traitMultiplDef_sum += characterData.trait_Tier2[selectTrait_Tier3 - 1].multiplyDef;
                    traitMultiplAtk_sum += characterData.trait_Tier2[selectTrait_Tier3 - 1].multiplyAtk;
                }
            }
        }

        //클래스의 경우 기본 클래스(1레벨부터 적용)의 값이 저장되고, 상위 클래스가 해금되었고 그 클래스를 선택했는지 체크해 상위 클래스 스탯 증가치를 합산한다.
        int classincrHp_sum = characterData.basicClass.increaseHealth;
        int classincrDef_sum = characterData.basicClass.increaseDef;
        int classincrAtk_sum = characterData.basicClass.increaseAtk;
        int classincrMov_sum = characterData.basicClass.increasecMov;

        float classIncrCtr_sum = characterData.basicClass.increasecCtr;
        float classIncrCtd_sum = characterData.basicClass.increasecCtd;
        float classIncrInfD_sum = characterData.basicClass.increaseInflictDamage;
        float classIncrTakenD_sum = characterData.basicClass.reducedTakenDamage;

        float classMultiplHp_sum = characterData.basicClass.multiplyHealth;
        float classMultiplDef_sum = characterData.basicClass.multiplyDef;
        float classMultiplAtk_sum = characterData.basicClass.multiplyAtk;

        if (selectSuperialClass != 0)
        {
            classincrHp_sum += characterData.superiorClass[selectSuperialClass - 1].increaseHealth;
            classincrDef_sum += characterData.superiorClass[selectSuperialClass - 1].increaseDef;
            classincrHp_sum += characterData.superiorClass[selectSuperialClass - 1].increaseAtk;
            classincrHp_sum += characterData.superiorClass[selectSuperialClass - 1].increasecMov;

            classIncrCtr_sum += characterData.superiorClass[selectSuperialClass - 1].increasecCtr;
            classIncrCtd_sum += characterData.superiorClass[selectSuperialClass - 1].increasecCtd;
            classIncrInfD_sum += characterData.superiorClass[selectSuperialClass - 1].increaseInflictDamage;
            classIncrTakenD_sum += characterData.superiorClass[selectSuperialClass - 1].reducedTakenDamage;

            classMultiplHp_sum = characterData.superiorClass[selectSuperialClass - 1].multiplyHealth;
            classMultiplHp_sum = characterData.superiorClass[selectSuperialClass - 1].multiplyDef;
            classMultiplHp_sum = characterData.superiorClass[selectSuperialClass - 1].multiplyAtk;
        }

        //이렇게 저장한 합계치를 Calc 스탯에 계산해서 저장한다.
        //계산식 간략히 : Calc 스탯 = (기본 스탯값 + 합연산 상수 수치값의 합) *(곱연산 배율 수치값의 합)
        //계산식 상세 : Calc 스탯 = (int)(  (기본 스탯값 + 특성으로 증가하는 스탯 합계치 + 클래스로 증가하는 스탯 합계치) * ( 1 + 특성으로 증가하는 스탯 배율 + 클래스로 증가하는 스탯 배율)  )
        CalcHealth = (int)((Health + traitIncrHp_sum + classincrHp_sum) * (1 + traitMultiplHp_sum + classMultiplHp_sum));
        CalcDef = (int)((Defence + traitIncrDef_sum + classincrDef_sum) * (1 + traitMultiplDef_sum + classMultiplDef_sum));
        CalcAtk = (int)((Attack + traitIncrAtk_sum + classincrAtk_sum) * (1 + traitMultiplAtk_sum + classMultiplAtk_sum));
        CalcMov = ((Mov + traitIncrMov_sum + classincrMov_sum));

        CalcCrtRate = (CriticalRate + traitIncrCtr_sum + classIncrCtr_sum);
        CalcCrtDMG = (CriticalDMG + traitIncrCtd_sum + classIncrCtd_sum);
        CalcInflictDMGRatio = (InflictDamageRatio + traitIncrInfD_sum + classIncrInfD_sum);
        CalcTakenDMGRatio = (TakenDamageRatio + traitIncrTakenD_sum + classIncrTakenD_sum);
        //기본 스탯 값과 Calc 스탯값이 분리되어있으므로, 실제 인게임 전투에서는 Calc스탯을 사용해주세요.
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

    //2티어 특성 선택 시 사용하는 메서드. UI와 연동 필요함
    public bool SelectTrait_tier2(int select)
    {
        if (select > 0 && select <= characterData.trait_Tier2.Length - 1)
        {
                if (level >= 50)
            {
                selectTrait_Tier2 = select;
                ApplyTraitClassStat();
                return true;
            }
            else
            {
                Debug.Log("2티어 특성 설정 조건을 충족하지 못했습니다.");
                return false;
            }
        }
        else
        {
            Debug.Log("잘못된 접근값입니다.");
            return false;
        }
    }
    //3티어 특성 선택 시 사용하는 메서드. UI와 연동 필요함
    public bool SelectTrait_tier3(int select)
    {
        if (select > 0 && select <= characterData.trait_Tier3.Length - 1)
        {
                if (level >= 70 && selectTrait_Tier2 != 0)
            {
                selectTrait_Tier3 = select;
                ApplyTraitClassStat();
                return true;
            }
            else
            {
                Debug.Log("3티어 특성 설정 조건을 충족하지 못했습니다.");
                return false;
            }
        }
        else
        {
            Debug.Log("잘못된 접근값입니다.");
            return false;
        }
    }

    //상위 클래스 선택 시 사용하는 메서드. UI와 연동 필요함.
    public bool SelectSuperialClass(int select)
    {
        if (select > 0 && select <= characterData.superiorClass.Length - 1)
        {
            if (level >= 60)
            {
                selectSuperialClass = select;
                ApplyTraitClassStat();
                return true;
            }
            else
            {
                Debug.Log("상위 클래스 설정 조건을 충족하지 못했습니다.");
                return false;
            }
        }
        else
        {
            Debug.Log("잘못된 접근값입니다.");
            return false;
        }
    }

    public int LimitBreak(int piece) //임시로 설정한 한계 돌파 메서드. 보유한 조각 개수를 매개변수로 받는다.
                                     //Todo: 조각 아이템 데이터를 받아와서 갯수를 체크하고, 돌파시 갯수 차감하도록 기능 구현하기.
    {
        if(star == Star.LimitBreak_4) //한계 돌파가 이미 최대치면 돌파 불가능.
        {
            return 0;
        }
        if (level == maxLevel && piece >= (int)star + 30) //현재 캐릭터 레벨이 최대치이고, 보유 조각 개수가 돌파 요건값 이상일 경우. (돌파 요건값 = 현재 최대레벨 + 30) 
        {
            int paidPiece = (int)star + 30;
            if ((int)star <= 70) //현재 성급이 5성 미만일 경우, 최대 레벨 10 증가.
            {
                star = (Star)((int)star + 10);
            }
            else //현재 성급이 5성 이상일 경우, 최대 레벨 5 증가.
            {
                star = (Star)((int)star + 5);
            }
            maxLevel = (int)star;
            return paidPiece; //차감해야하는 조각 값을 리턴한다.
        }
        else
        {
            //돌파 요건 미충족.
            return 0;
        }
    }
}
