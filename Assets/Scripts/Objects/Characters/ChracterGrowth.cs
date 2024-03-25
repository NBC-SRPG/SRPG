using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Constants;

public class CharacterGrowth : MonoBehaviour
{
    private Character character;

    //'현재 선택중인 특성 번호' ( 0 = None )
    //이 변수들을 설정해 배열의 index 번호를 지정한다.
    public int selectTalent_Tier2 { get; private set; }
    public int selectTalent_Tier3 { get; private set; }
    public int selectSuperialClass { get; private set; }


    //'현재 선택 중인 특성 번호의 특성/클래스의 SO 를 참조함' 
    public TalentSO talent_Tier1 { get; private set; }
    public TalentSO talent_Tier2 { get; private set; }
    public TalentSO talent_Tier3 { get; private set; }

    public ClassSO basicClass { get; private set; }
    public ClassSO superiorClass { get; private set; }


    //레벨, 최대레벨, 경험치, 최대 경험치
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            if (Managers.AccountData.playerData.Level >= maxLevel) 
            {
                level = math.clamp(value, 1, maxLevel);
            }
            else
            {
                level = math.clamp(value, 1, Managers.AccountData.playerData.Level);
            }
            ApplyGrowStat();
            ApplyAdditionStat(); //레벨업 시 성장 스탯 및 특성/클래스 스탯 리프레시.
            maxExp = (level * 100 + maxLevel * level * 20); //임의로 최대 경험치값(레벨업하는데 필요한 경험치 값)을 설정. 
        }
    }

    public int maxLevel { get; private set; }

    private int exp;
    public int Exp
    {
        get { return exp; }
        private set //경험치 설정자에 자체적으로 레벨업 기능을 배치했습니다.
        {
            exp = value;
            while (exp >= maxExp && level < maxLevel && level < Managers.AccountData.playerData.Level)  //레벨이 계정 레벨을 넘지 못하도록 조건 추가
            {
                exp -= maxExp;
                level += 1;
            }
            exp = math.clamp(exp, 0, maxExp);
        }
    }

    public int maxExp { get; private set; }

    public Star star { get; private set; } //성급

    public int ExSkillLevel { get; private set; } //Ex스킬 레벨 //Todo: 스킬 레벨에 따라 스킬 계수 적용시키기, 실제 인게임에서 스킬 레벨에 따라 효과 달라지게 하기.
    public int PassiveSkillLevel { get; private set; } //패시브 스킬 레벨

    public int weaponLevel { get; private set; } //무기 레벨
    public int armorLevel { get; private set; } //방어구 레벨
    public int affectionLevel { get; private set; } //호감도 레벨


    //CharacterGrowth에 Character을 넣고 선택중인 특성과 클래스를 CharacterData의 SO와 연결시키는 메서드.
    public void Init(Character _character)
    {
        character = _character;
        talent_Tier1 = character.characterData.talent_Tier1;
        talent_Tier2 = character.characterData.talent_Tier2[selectTalent_Tier2 -1];
        talent_Tier3 = character.characterData.talent_Tier3[selectTalent_Tier3 -1];

        basicClass = character.characterData.basicClass;
        superiorClass = character.characterData.superiorClass[selectSuperialClass -1];
    }

    //최초 초기화 메서드
    //캐릭터를 계정에서 최초로 획득 시 이 메서드를 호출할 것.
    public void InitialInit() 
    {
        star = character.characterData.defaltStar; //캐릭터의 성급은 획득 시 기본 성급.
        Level = 1;
        maxLevel = (int)star; //최대 레벨은 기본 성급에 의해 정해짐.
        weaponLevel = 1;
        armorLevel = 1;
        ExSkillLevel = 1;
        PassiveSkillLevel = 1; 
        affectionLevel = 1;
    }

    //2티어 특성 선택 시 사용하는 메서드. UI와 연동 필요함
    public bool SelectTalent_tier2(int select)
    {
        if (select > 0 && select <= character.characterData.talent_Tier2.Length - 1)
        {
            if (Level >= 50)
            {
                selectTalent_Tier2 = select;
                ApplyAdditionStat();
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
    public bool SelectTalent_tier3(int select)
    {
        if (select > 0 && select <= character.characterData.talent_Tier3.Length - 1)
        {
            if (level >= 70 && selectTalent_Tier2 != 0)
            {
                selectTalent_Tier3 = select;
                ApplyAdditionStat();
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
        if (select > 0 && select <= character.characterData.superiorClass.Length - 1)
        {
            if (level >= 60)
            {
                selectSuperialClass = select;
                ApplyAdditionStat();
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

    public int LimitBreak(int piece) //임시로 설정한 한계 돌파 메서드. 캐릭터 조각 갯수 체크 및 조각 소모 기능 구현 필요.
                                     //Todo: 조각 아이템 데이터를 받아와서 갯수를 체크하고, 돌파시 갯수 차감하도록 기능 구현하기.
    {
        if (star == Star.LimitBreak_4) //한계 돌파가 이미 최대치면 돌파 불가능.
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

    public void ApplyGrowStat() //캐릭터 성장 스탯(레벨) 적용 메서드. 레벨값이 바뀔 때마다 호출된다. 계산한 뒤 Character의 기본 스탯에 저장한다. 
    {
        //캐릭터의 스탯 값을 (기본 스탯값 + 레벨에 의해 증가한 수치값)으로 저장한다.
        //계산식 : 스탯값 = 원본캐릭터의 기본 스탯 + (원본 캐릭터의 성장 스탯 * 이 캐릭터 개체의 현재 레벨)
        character.Health = (character.characterData.health + (character.characterData.growHealth * level));
        character.Attack = (character.characterData.atk + (character.characterData.growAtk * level));
        character.Defence = (character.characterData.def + (character.characterData.growDef * level));
    }

    public void ApplyAdditionStat() //캐릭터 특성 / 클래스 / 장비 스탯 적용 메서드, 계산한 뒤 값을 Character의 Calc 스탯에 저장한다. 
    {
        //특성으로 올라가는 능력치들의 합계를 저장할 변수를 선언한다.
        int talentIncrHp_sum = 0; //체력 상수 증가치 합계
        int talentIncrDef_sum = 0; //방어력
        int talentIncrAtk_sum = 0; //공격력
        int talentIncrMov_sum = 0; //이동거리

        float talentIncrCtr_sum = 0; //치명타 확률 ( 수치 0.1 = 치명타 확률 10%증가)
        float talentIncrCtd_sum = 0; //치명타 피해
        float talentIncrInfD_sum = 0; //주는 데미지 증가
        float talentIncrTakenD_sum = 0; //받는 피해 감소

        float talentMultiplHp_sum = 0; //체력 배율 증가치 합계
        float talentMultiplDef_sum = 0; //방어력 배율 증가치 합계
        float talentMultiplAtk_sum = 0; //공격력 배율 증가치 합계

        if (level >= 30) //레벨이 30 이상이어서 1차 특성이 해금되었는지 확인한다.
        {
            //티어1 특성은 하나 뿐이므로, 캐릭터SO가 가진 1티어 특성의 증가값을 가져와 합계에 저장한다.
            talentIncrHp_sum = talent_Tier1.increaseHealth;
            talentIncrDef_sum = talent_Tier1.increaseDef;
            talentIncrAtk_sum = talent_Tier1.increaseAtk;
            talentIncrMov_sum = talent_Tier1.increasecMov;

            talentIncrCtr_sum = talent_Tier1.increasecCtr;
            talentIncrCtd_sum = talent_Tier1.increasecCtd;
            talentIncrInfD_sum = talent_Tier1.increaseInflictDamage;
            talentIncrTakenD_sum = talent_Tier1.reducedTakenDamage;

            talentMultiplHp_sum = talent_Tier1.multiplyHealth;
            talentMultiplDef_sum = talent_Tier1.multiplyDef;
            talentMultiplAtk_sum = talent_Tier1.multiplyAtk;

            if (talent_Tier2 != null) //'선택한 2티어 특성'값이 0이라면 특성이 개방되지 않았거나 아직 선택하지 않은 경우.
            {
                //selectTrait_Tier 값이 0이 아니라면 특성을 선택했다는 뜻. 캐릭터SO가 가진 '선택가능한 티어 2 특성 배열'에서 일치하는 특성을 가져와 그 증가값을 합계에 더한다. 
                talentIncrHp_sum += talent_Tier2.increaseHealth;
                talentIncrDef_sum += talent_Tier2.increaseDef;
                talentIncrAtk_sum += talent_Tier2.increaseAtk;
                talentIncrMov_sum += talent_Tier2.increasecMov;

                talentIncrCtr_sum += talent_Tier2.increasecCtr;
                talentIncrCtd_sum += talent_Tier2.increasecCtd;
                talentIncrInfD_sum += talent_Tier2.increaseInflictDamage;
                talentIncrTakenD_sum += talent_Tier2.reducedTakenDamage;

                talentMultiplHp_sum += talent_Tier2.multiplyHealth;
                talentMultiplDef_sum += talent_Tier2.multiplyDef;
                talentMultiplAtk_sum += talent_Tier2.multiplyAtk;
                if (talent_Tier3 != null)
                {
                    //3티어 특성도 마찬가지로 적용된다.
                    talentIncrHp_sum += talent_Tier3.increaseHealth;
                    talentIncrDef_sum += talent_Tier3.increaseDef;
                    talentIncrAtk_sum += talent_Tier3.increaseAtk;
                    talentIncrMov_sum += talent_Tier3.increasecMov;

                    talentIncrCtr_sum += talent_Tier3.increasecCtr;
                    talentIncrCtd_sum += talent_Tier3.increasecCtd;
                    talentIncrInfD_sum += talent_Tier3.increaseInflictDamage;
                    talentIncrTakenD_sum += talent_Tier3.reducedTakenDamage;

                    talentMultiplHp_sum += talent_Tier2.multiplyHealth;
                    talentMultiplDef_sum += talent_Tier2.multiplyDef;
                    talentMultiplAtk_sum += talent_Tier2.multiplyAtk;
                }
            }
        }

        //클래스의 경우 기본 클래스(1레벨부터 적용)의 값이 저장되고, 상위 클래스가 해금되었고 그 클래스를 선택했는지 체크해 상위 클래스 스탯 증가치를 합산한다.
        int classincrHp_sum = basicClass.increaseHealth;
        int classincrDef_sum = basicClass.increaseDef;
        int classincrAtk_sum = basicClass.increaseAtk;
        int classincrMov_sum = basicClass.increasecMov;

        float classIncrCtr_sum = basicClass.increasecCtr;
        float classIncrCtd_sum = basicClass.increasecCtd;
        float classIncrInfD_sum = basicClass.increaseInflictDamage;
        float classIncrTakenD_sum = basicClass.reducedTakenDamage;

        float classMultiplHp_sum = basicClass.multiplyHealth;
        float classMultiplDef_sum = basicClass.multiplyDef;
        float classMultiplAtk_sum = basicClass.multiplyAtk;

        if (superiorClass != null)
        {
            classincrHp_sum += superiorClass.increaseHealth;
            classincrDef_sum += superiorClass.increaseDef;
            classincrHp_sum += superiorClass.increaseAtk;
            classincrHp_sum += superiorClass.increasecMov;

            classIncrCtr_sum += superiorClass.increasecCtr;
            classIncrCtd_sum += superiorClass.increasecCtd;
            classIncrInfD_sum += superiorClass.increaseInflictDamage;
            classIncrTakenD_sum += superiorClass.reducedTakenDamage;

            classMultiplHp_sum = superiorClass.multiplyHealth;
            classMultiplHp_sum = superiorClass.multiplyDef;
            classMultiplHp_sum = superiorClass.multiplyAtk;
        }

        //장비 스탯.
        //무기로 획득할 수 있는 스탯 = 공격력, 치확, 치피, 주는 피해증가
        //방어구로 획득할 수 있는 스탯 = 체력, 방어력, 받는 피해 감소
        int weaponincrAtk_sum = (character.characterData.weapon.increaseAtk * weaponLevel) ;
        float weaponMultiplAtk_sum = (character.characterData.weapon.multiplyAtk * weaponLevel);
        float weaponIncrCtr_sum = (character.characterData.weapon.increasecCtr * weaponLevel);
        float weaponIncrCtd_sum = (character.characterData.weapon.increasecCtd * weaponLevel);
        float weaponIncrInfD_sum = (character.characterData.weapon.increaseInflictDamage * weaponLevel);

        int armorincrHp_sum = (character.characterData.armor.increaseHealth * armorLevel);
        int armorincrDef_sum = (character.characterData.armor.increaseDef * armorLevel);
        float armorMultiplHp_sum = (character.characterData.armor.multiplyHealth * armorLevel);
        float armorMultiplDef_sum = (character.characterData.armor.multiplyDef * armorLevel);
        float armorIncrTakenD_sum = (character.characterData.armor.reducedTakenDamage * armorLevel);


        //이렇게 저장한 합계치를 Calc 스탯에 계산해서 저장한다.
        //계산식 간략히 : Calc 스탯 = (기본 스탯값 + 합연산 상수 수치값의 합) *(곱연산 배율 수치값의 합)
        //계산식 상세 : Calc 스탯 = (int)(  (기본 스탯값 + 특성으로 증가하는 스탯 합계치 + 클래스로 증가하는 스탯 합계치) * ( 1 + 특성으로 증가하는 스탯 배율 + 클래스로 증가하는 스탯 배율)  )
        character.CalcHealth = (int)((character.Health + talentIncrHp_sum + classincrHp_sum + armorincrHp_sum) * (1 + talentMultiplHp_sum + classMultiplHp_sum + armorMultiplHp_sum));
        character.CalcDef = (int)((character.Defence + talentIncrDef_sum + classincrDef_sum + armorincrDef_sum) * (1 + talentMultiplDef_sum + classMultiplDef_sum + armorMultiplDef_sum));
        character.CalcAtk = (int)((character.Attack + talentIncrAtk_sum + classincrAtk_sum + weaponincrAtk_sum) * (1 + talentMultiplAtk_sum + classMultiplAtk_sum + weaponMultiplAtk_sum));
        character.CalcMov = ((character.Mov + talentIncrMov_sum + classincrMov_sum));

        character.CalcCrtRate = (character.CriticalRate + talentIncrCtr_sum + classIncrCtr_sum + weaponIncrCtr_sum);
        character.CalcCrtDMG = (character.CriticalDMG + talentIncrCtd_sum + classIncrCtd_sum + weaponIncrCtd_sum);
        character.CalcInflictDMGRatio = (character.InflictDamageRatio + talentIncrInfD_sum + classIncrInfD_sum + weaponIncrInfD_sum);
        character.CalcTakenDMGRatio = (character.TakenDamageRatio + talentIncrTakenD_sum + classIncrTakenD_sum + armorIncrTakenD_sum);
        //기본 스탯 값과 Calc 스탯값이 분리되어있으므로, 실제 인게임 전투에서는 Calc스탯을 사용해주세요.
    }

    public bool WeaponLevelUp(int ingredient)
    {
        if (ingredient >= weaponLevel && weaponLevel < level)
        {
            weaponLevel += 1;
            //Todo : 재료 소모
            return true;
        }
        else
        {
            return false;
        }
    }//임시로 구조만 만들어둔 무기 레벨업 메서드. //Todo: 장비 레벨업 방식 및 요구 아이템. 장비 등급 상승, 장비 레벨업 시 스탯 관련 논의 필요.

    public bool ArmorLevelUp(int ingredient)
    {
        if (ingredient >= weaponLevel && weaponLevel < level)
        {
            armorLevel += 1;
            //Todo : 재료 소모
            return true;
        }
        else
        {
            return false;
        }
    }//임시로 구조만 만들어둔 방어구 레벨업 메서드. //Todo: 위와 같음.

    public bool ExSkillLevelUp(int ingredient)
    {
        if (ingredient >= ExSkillLevel && ExSkillLevel < 5)
        {
            ExSkillLevel += 1;
            //Todo : 재료 소모
            return true;
        }
        else
        {
            return false;
        }
    }//임시로 구조만 만들어둔 스킬 레벨업 메서드, //Todo: 스킬 레벨업 재화 소모, 스킬 레벨업 시 효과 등

    public bool AffectionLevelUp(int ingredient)
    {
        if (ingredient >= affectionLevel && affectionLevel < 99)
        {
            affectionLevel += 1;
            //Todo : 재료 소모
            return true;
        }
        else
        {
            return false;
        }
    }//임시로 구조만 만들어둔 호감도 레벨업 메서드, //Todo: 호감도 레벨업 재화 소모, 호감도 증가 시 추가 효과 등.
}
