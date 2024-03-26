using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Constants;
using static RankUp_skillMaterials;

public class CharacterGrowthManager //캐릭터 성장 / 특성 및 클래스 선택 / 스탯 계산 메서드 등 데이터가 아닌 메서드만을 따로 저장하는 클래스.
{
    private Character character;
    public void Init(Character _character)
    {
        character = _character;
    }

    //2티어 특성 선택 시 사용하는 메서드. UI와 연동 필요함
    public void SelectTalent_tier2(TalentSO select)
    {
        character.characterGrowth.talent_Tier2 = select;
        ApplyAdditionStat();
    }

    //3티어 특성 선택 시 사용하는 메서드. UI와 연동 필요함
    public void SelectTalent_tier3(TalentSO select)
    {
        character.characterGrowth.talent_Tier3 = select;
        ApplyAdditionStat();
    }

    //상위 클래스 선택 시 사용하는 메서드. UI와 연동 필요함.
    public void SelectSuperialClass(ClassSO select)
    {
        character.characterGrowth.superiorClass = select;
        ApplyAdditionStat();
    }

    //임시로 틀만 갖춰둔 Exp 증가 메서드.
    //Todo: 캐릭터 정보 창에서 캐릭터 경험치 아이템을 선택하고, 그 갯수를 조정하고, 그 갯수만큼 경험치를 합산해서, 경험치를 올리고 선택한 경험치 아이템들의 갯수를 차감하도록 기능 구현하기.
    //캐릭터 경험치 아이템은 일단 등급 별 4종이 존재하므로, UI에서 인벤토리에 있는 경험치 아이템 4종의 갯수와 아이콘을 띄우고 플레이어가 해당 등급의 아이템 아이콘 터치 시 해당 아이템의 선택한 갯수 값이 1 증가하도록 저장해야함.
    //레퍼런스: 블루 아카이브의 캐릭터 육성창-레벨업 부분 참조
    public void CharacterExpAdd(int sum)
    {
        character.characterGrowth.Exp += sum;
    }


    public void LimitBreak() //조각을 소모하고 돌파를 진행하는 메서드.
    {
        if (character.characterGrowth.Limit >= 4) //한계 돌파가 이미 최대치면 돌파 불가능.
        {
            return;
        }

        //돌파 조건문 해설
        //조건 1: 캐릭터 레벨이 캐릭터의 최대 레벨과 같은지?
        //조건 2: 어카운트 데이터에서 아이템 갯수를 리턴하는 메서드를 이용, 캐릭터 ID와 같은 ID 값을 갖는 아이템의 갯수를 체크한다. 그 값이 캐릭터의 최대레벨 + 30 이상인지?
        if (character.characterGrowth.Level == character.characterGrowth.maxLevel && Managers.AccountData.GetItemQuantity(character.characterData.character_Id) >= character.characterGrowth.maxLevel + 30) //현재 캐릭터 레벨이 최대치이고, 보유 조각 개수가 돌파 요건값 이상일 경우. (돌파 요건값 = 현재 최대레벨 + 30) 
        {
            //돌파 진행 후, 조각을 요구 갯수만큼 차감. (Star, Limit를 증가시키면 MaxLevel 값도 같이 증가하므로, 코드 서순에 주의)
            Managers.AccountData.UseItem(character.characterData.character_Id, character.characterGrowth.maxLevel + 30);

            if (character.characterGrowth.Star < 5) //현재 성급이 5성 미만일 경우, 최대 레벨 10 증가.
            {
                character.characterGrowth.Star += 1;
            }
            else if (character.characterGrowth.Star >= 5 && character.characterGrowth.Limit < 4) //현재 성급이 5성 이상이고 한계가 4 미만일 경우, 최대 레벨 5 증가.
            {
                character.characterGrowth.Limit += 1; 
            }
        }
        else
        {
        }
    }

    public void ApplyGrowStat() //캐릭터 성장 스탯(레벨) 적용 메서드. 레벨값이 바뀔 때마다 호출된다. 계산한 뒤 Character의 기본 스탯에 저장한다. 
    {
        //캐릭터의 스탯 값을 (기본 스탯값 + 레벨에 의해 증가한 수치값)으로 저장한다.
        //계산식 : 스탯값 = 원본캐릭터의 기본 스탯 + (원본 캐릭터의 성장 스탯 * 이 캐릭터 개체의 현재 레벨)
        character.Health = (character.characterData.health + (character.characterData.growHealth * character.characterGrowth.Level));
        character.Attack = (character.characterData.atk + (character.characterData.growAtk * character.characterGrowth.Level));
        character.Defence = (character.characterData.def + (character.characterData.growDef * character.characterGrowth.Level));
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

        if (character.characterGrowth.Level >= 30) //레벨이 30 이상이어서 1차 특성이 해금되었는지 확인한다.
        {
            //티어1 특성은 하나 뿐이므로, 캐릭터SO가 가진 1티어 특성의 증가값을 가져와 합계에 저장한다.
            talentIncrHp_sum = character.characterGrowth.talent_Tier1.increaseHealth;
            talentIncrDef_sum = character.characterGrowth.talent_Tier1.increaseDef;
            talentIncrAtk_sum = character.characterGrowth.talent_Tier1.increaseAtk;
            talentIncrMov_sum = character.characterGrowth.talent_Tier1.increasecMov;

            talentIncrCtr_sum = character.characterGrowth.talent_Tier1.increasecCtr;
            talentIncrCtd_sum = character.characterGrowth.talent_Tier1.increasecCtd;
            talentIncrInfD_sum = character.characterGrowth.talent_Tier1.increaseInflictDamage;
            talentIncrTakenD_sum = character.characterGrowth.talent_Tier1.reducedTakenDamage;

            talentMultiplHp_sum = character.characterGrowth.talent_Tier1.multiplyHealth;
            talentMultiplDef_sum = character.characterGrowth.talent_Tier1.multiplyDef;
            talentMultiplAtk_sum = character.characterGrowth.talent_Tier1.multiplyAtk;

            if (character.characterGrowth.talent_Tier2 != null) //'선택한 2티어 특성'값이 0이라면 특성이 개방되지 않았거나 아직 선택하지 않은 경우.
            {
                //selectTrait_Tier 값이 0이 아니라면 특성을 선택했다는 뜻. 캐릭터SO가 가진 '선택가능한 티어 2 특성 배열'에서 일치하는 특성을 가져와 그 증가값을 합계에 더한다. 
                talentIncrHp_sum += character.characterGrowth.talent_Tier2.increaseHealth;
                talentIncrDef_sum += character.characterGrowth.talent_Tier2.increaseDef;
                talentIncrAtk_sum += character.characterGrowth.talent_Tier2.increaseAtk;
                talentIncrMov_sum += character.characterGrowth.talent_Tier2.increasecMov;

                talentIncrCtr_sum += character.characterGrowth.talent_Tier2.increasecCtr;
                talentIncrCtd_sum += character.characterGrowth.talent_Tier2.increasecCtd;
                talentIncrInfD_sum += character.characterGrowth.talent_Tier2.increaseInflictDamage;
                talentIncrTakenD_sum += character.characterGrowth.talent_Tier2.reducedTakenDamage;

                talentMultiplHp_sum += character.characterGrowth.talent_Tier2.multiplyHealth;
                talentMultiplDef_sum += character.characterGrowth.talent_Tier2.multiplyDef;
                talentMultiplAtk_sum += character.characterGrowth.talent_Tier2.multiplyAtk;
                if (character.characterGrowth.talent_Tier3 != null)
                {
                    //3티어 특성도 마찬가지로 적용된다.
                    talentIncrHp_sum += character.characterGrowth.talent_Tier3.increaseHealth;
                    talentIncrDef_sum += character.characterGrowth.talent_Tier3.increaseDef;
                    talentIncrAtk_sum += character.characterGrowth.talent_Tier3.increaseAtk;
                    talentIncrMov_sum += character.characterGrowth.talent_Tier3.increasecMov;

                    talentIncrCtr_sum += character.characterGrowth.talent_Tier3.increasecCtr;
                    talentIncrCtd_sum += character.characterGrowth.talent_Tier3.increasecCtd;
                    talentIncrInfD_sum += character.characterGrowth.talent_Tier3.increaseInflictDamage;
                    talentIncrTakenD_sum += character.characterGrowth.talent_Tier3.reducedTakenDamage;

                    talentMultiplHp_sum += character.characterGrowth.talent_Tier2.multiplyHealth;
                    talentMultiplDef_sum += character.characterGrowth.talent_Tier2.multiplyDef;
                    talentMultiplAtk_sum += character.characterGrowth.talent_Tier2.multiplyAtk;
                }
            }
        }

        //클래스의 경우 기본 클래스(1레벨부터 적용)의 값이 저장되고, 상위 클래스가 해금되었고 그 클래스를 선택했는지 체크해 상위 클래스 스탯 증가치를 합산한다.
        int classincrHp_sum = character.characterGrowth.basicClass.increaseHealth;
        int classincrDef_sum = character.characterGrowth.basicClass.increaseDef;
        int classincrAtk_sum = character.characterGrowth.basicClass.increaseAtk;
        int classincrMov_sum = character.characterGrowth.basicClass.increasecMov;

        float classIncrCtr_sum = character.characterGrowth.basicClass.increasecCtr;
        float classIncrCtd_sum = character.characterGrowth.basicClass.increasecCtd;
        float classIncrInfD_sum = character.characterGrowth.basicClass.increaseInflictDamage;
        float classIncrTakenD_sum = character.characterGrowth.basicClass.reducedTakenDamage;

        float classMultiplHp_sum = character.characterGrowth.basicClass.multiplyHealth;
        float classMultiplDef_sum = character.characterGrowth.basicClass.multiplyDef;
        float classMultiplAtk_sum = character.characterGrowth.basicClass.multiplyAtk;

        if (character.characterGrowth.superiorClass != null)
        {
            classincrHp_sum += character.characterGrowth.superiorClass.increaseHealth;
            classincrDef_sum += character.characterGrowth.superiorClass.increaseDef;
            classincrHp_sum += character.characterGrowth.superiorClass.increaseAtk;
            classincrHp_sum += character.characterGrowth.superiorClass.increasecMov;

            classIncrCtr_sum += character.characterGrowth.superiorClass.increasecCtr;
            classIncrCtd_sum += character.characterGrowth.superiorClass.increasecCtd;
            classIncrInfD_sum += character.characterGrowth.superiorClass.increaseInflictDamage;
            classIncrTakenD_sum += character.characterGrowth.superiorClass.reducedTakenDamage;

            classMultiplHp_sum += character.characterGrowth.superiorClass.multiplyHealth;
            classMultiplDef_sum += character.characterGrowth.superiorClass.multiplyDef;
            classMultiplAtk_sum += character.characterGrowth.superiorClass.multiplyAtk;
        }

        //장비 스탯.
        //무기로 획득할 수 있는 스탯 = 공격력, 치확, 치피, 주는 피해증가
        //방어구로 획득할 수 있는 스탯 = 체력, 방어력, 받는 피해 감소
        int weaponincrAtk_sum = (character.characterGrowth.weapon.increaseAtk * (character.characterGrowth.weaponEnhance + 1));
        float weaponMultiplAtk_sum = (character.characterGrowth.weapon.multiplyAtk * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrCtr_sum = (character.characterGrowth.weapon.increasecCtr * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrCtd_sum = (character.characterGrowth.weapon.increasecCtd * (character.characterGrowth.weaponEnhance + 1));
        float weaponIncrInfD_sum = (character.characterGrowth.weapon.increaseInflictDamage * (character.characterGrowth.weaponEnhance + 1));

        int armorincrHp_sum = (character.characterGrowth.armor.increaseHealth * (character.characterGrowth.armorEnhance + 1));
        int armorincrDef_sum = (character.characterGrowth.armor.increaseDef * (character.characterGrowth.armorEnhance + 1));
        float armorMultiplHp_sum = (character.characterGrowth.armor.multiplyHealth * (character.characterGrowth.armorEnhance + 1));
        float armorMultiplDef_sum = (character.characterGrowth.armor.multiplyDef * (character.characterGrowth.armorEnhance + 1));
        float armorIncrTakenD_sum = (character.characterGrowth.armor.reducedTakenDamage * (character.characterGrowth.armorEnhance + 1));


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

    public void WeaponRankUp()
    {
        //장비 강화 조건
        //0~4강은 캐릭터가 어떤 레벨이든 강화 가능
        //4강에서 +1강 하는 것은 다음 등급 장비로 전환을 의미함.
        //캐릭터 0~30레벨 : 커먼 등급 이하 장비 착용 가능
        //캐릭터 30~50레벨 : 레어 등급 이하 장비 착용 가능
        //캐릭터 50~70레벨 : 에픽 등급 이하 장비 착용 가능
        //캐릭터 70~90레벨 : 레전드 등급 이하 장비 착용 가능.
        //따라서, 강화 단계가 4일 때에는 캐릭터 레벨이 적정 레벨 이상이어야 다음 등급 장비로 강화할 수 있다.

        if(character.characterGrowth.weaponTier >= (int)ItemRank.Legend) //레전드 등급 이상일 경우 강화 불가. (전용무기는 Myth 등급이지만 구현예정 x)
        {
            return;
        }

        if (character.characterGrowth.weaponEnhance >= 4) //캐릭터의 장비 강화 정도가 4 이상일 경우
        {
            if (character.characterGrowth.Level < (character.characterGrowth.weaponTier * 20 + 30)) //적정 레벨인지 체크하는 조건문. 현재 장비 등급이 0티어(커먼)일 때 캐릭터 레벨이 30 미만이면 강화 불가.
            {
                return;
            }
        }

        // rankUpMaterials의 모든 요소에 대해 반복
        foreach (var kvp in character.characterGrowth.weapon.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value * (character.characterGrowth.weaponEnhance); // 요구되는 아이템의 갯수 = 강화 정도마다 요구량이 증가한다.

            // 인벤토리에 해당 아이템이 없거나 갯수가 요구되는 갯수보다 적은 경우
            if (!Managers.AccountData.inventory.TryGetValue(requiredItemId, out int currentItemCount) ||
                currentItemCount < requiredItemCount)
            {
                // 요구 사항을 만족하지 못하면 종료
                return;
            }
        }

        // 모든 요구 사항을 만족하는 경우 강화 성공
        // 강화에 필요한 강화 소재 아이템들을 차감
        foreach (var kvp in character.characterGrowth.weapon.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value * (character.characterGrowth.weaponEnhance); // 요구되는 아이템의 갯수

            // 인벤토리에서 해당 아이템 갯수 차감
            Managers.AccountData.RemoveItem(requiredItemId, requiredItemCount);
        }
        
        if (character.characterGrowth.weaponEnhance >= 4) //캐릭터의 무기 강화 정도가 4 이상일 경우
        {
            character.characterGrowth.weaponTier += 1; //티어 1단계 Up하고 강화도 초기화,
            character.characterGrowth.weaponEnhance = 0;
        }
        else
        {
            character.characterGrowth.weaponEnhance += 1;
        }

        ApplyAdditionStat();
        return;
    }

    public void ArmorRankUp()
    {
        //장비 강화 조건
        //0~4강은 캐릭터가 어떤 레벨이든 강화 가능
        //4강에서 +1강 하는 것은 다음 등급 장비로 전환을 의미함.
        //캐릭터 0~30레벨 : 커먼 등급 이하 장비 착용 가능
        //캐릭터 30~50레벨 : 레어 등급 이하 장비 착용 가능
        //캐릭터 50~70레벨 : 에픽 등급 이하 장비 착용 가능
        //캐릭터 70~90레벨 : 레전드 등급 이하 장비 착용 가능.
        //따라서, 강화 단계가 4일 때에는 캐릭터 레벨이 적정 레벨 이상이어야 다음 등급 장비로 강화할 수 있다.

        if (character.characterGrowth.armorTier >= (int)ItemRank.Legend) //레전드 등급 이상일 경우 강화 불가. (전용무기는 Myth 등급이지만 구현예정 x)
        {
            return;
        }

        if (character.characterGrowth.armorEnhance >= 4) //캐릭터의 장비 강화 정도가 4 이상일 경우
        {
            if (character.characterGrowth.Level < (character.characterGrowth.armorTier * 20 + 30)) //적정 레벨인지 체크하는 조건문. 현재 장비 등급이 0티어(커먼)일 때 캐릭터 레벨이 30 미만이면 강화 불가.
            {
                return;
            }
        }

        // rankUpMaterials의 모든 요소에 대해 반복
        foreach (var kvp in character.characterGrowth.armor.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value * (character.characterGrowth.armorEnhance); // 요구되는 아이템의 갯수 = 강화 정도마다 요구량이 증가한다.

            // 인벤토리에 해당 아이템이 없거나 갯수가 요구되는 갯수보다 적은 경우
            if (!Managers.AccountData.inventory.TryGetValue(requiredItemId, out int currentItemCount) ||
                currentItemCount < requiredItemCount)
            {
                // 요구 사항을 만족하지 못하면 종료
                return;
            }
        }

        // 모든 요구 사항을 만족하는 경우 강화 성공
        // 강화에 필요한 강화 소재 아이템들을 차감
        foreach (var kvp in character.characterGrowth.armor.rankUpMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value * (character.characterGrowth.armorEnhance); // 요구되는 아이템의 갯수

            // 인벤토리에서 해당 아이템 갯수 차감
            Managers.AccountData.RemoveItem(requiredItemId, requiredItemCount);
        }

        if (character.characterGrowth.armorEnhance >= 4) //캐릭터의 무기 강화 정도가 4 이상일 경우
        {
            character.characterGrowth.armorTier += 1; //티어 1단계 Up하고 강화도 초기화,
            character.characterGrowth.armorEnhance = 0;
        }
        else
        {
            character.characterGrowth.armorEnhance += 1;
        }

        ApplyAdditionStat();
        return;
    }



    public void ExSkillLevelUp()
    {
        //스킬 강화 조건

        if (character.characterGrowth.ExSkillLevel >= 5) //스킬 5레벨 이상일 경우 강화 불가.
        {
            return;
        }

        //Todo: 스킬 레벨 별로 필요한 재료 데이터를 가져와야함
        //스킬 레벨은 진영 별 스킬 랭크업 아이템을 사용해서 레벨업한다.
        //진영 별 / 레벨 별로 필요 아이템 ID - 아이템 갯수 딕셔너리가 필요함
        //예시) 마족진영 스킬 레벨업 1→2에 필요한 딕셔너리<아이템 종류, 아이템 갯수>
        // 이런 식으로 마족, 2→3 레벨... 3→4 레벨.., 교단 1→2 레벨 / ...
        //일단 임시로 미리 만들어둔 static 딕셔너리의 데이터를 가져오는 형식을 채용함. DB 작업 후 DB 데이터를 가져오는 형식으로 변경할 필요 있음.

        // 스킬 레벨에 필요한 재료 가져오기
        // 메서드 GetSkillMaterials( 캐릭터 소속 진영, 캐릭터 스킬 레벨) 은 진영과 스킬 레벨에 따라서 해당 스킬 레벨업에 필요한 아이템 ID & 갯수 딕셔너리를 리턴함.
        Dictionary<int, int> requiredMaterials = RankUp_skillMaterials.GetSkillMaterials(character.characterData.belonging, character.characterGrowth.ExSkillLevel);
        if (requiredMaterials == null)
        {
            // 필요한 재료가 없는 경우 종료
            return;
        }


        foreach (var kvp in requiredMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value; // 요구되는 아이템의 갯수 = 스킬 레벨 정도마다 요구량이 증가한다.

            // 인벤토리에 해당 아이템이 없거나 갯수가 요구되는 갯수보다 적은 경우
            if (!Managers.AccountData.inventory.TryGetValue(requiredItemId, out int currentItemCount) ||
                currentItemCount < requiredItemCount)
            {
                // 요구 사항을 만족하지 못하면 종료
                return;
            }
        }

        // 모든 요구 사항을 만족하는 경우 스킬 강화 성공
        // 강화에 필요한 강화 소재 아이템들을 차감
        foreach (var kvp in requiredMaterials)
        {
            int requiredItemId = kvp.Key; // 요구되는 아이템의 아이디
            int requiredItemCount = kvp.Value; // 요구되는 아이템의 갯수

            // 인벤토리에서 해당 아이템 갯수 차감
            Managers.AccountData.RemoveItem(requiredItemId, requiredItemCount);
        }
        character.characterGrowth.ExSkillLevel += 1;
        return;
        
    }



    /* 호감도 메서드는 주석 처리. 시간이 남는대로 작업. 
    public bool AffectionLevelUp(int ingredient)
    {
        if (ingredient >= character.characterGrowth.affectionLevel && character.characterGrowth.affectionLevel < 99)
        {
            character.characterGrowth.affectionLevel += 1;
            //Todo : 재료 소모
            return true;
        }
        else
        {
            return false;
        }
    }//임시로 구조만 만들어둔 호감도 레벨업 메서드, //Todo: 호감도 레벨업 재화 소모, 호감도 증가 시 추가 효과 등.
    */
}
