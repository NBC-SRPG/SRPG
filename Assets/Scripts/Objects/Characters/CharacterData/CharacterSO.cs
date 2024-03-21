using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public Constants.AttackType attackType;//공격 타입
    public Constants.CharacterAttribute characterAttribute;//캐릭터 속성

    [Header("Develope")]
    public int character_Id;//캐릭터 식별자

    [Header("Status")]///캐릭터 능력치
    public int health;
    public int atk;
    public int def;
    public int mov;
    [Range(0, 10)]public int atk_range;//공격 사정거리(근거리의 경우 0으로)

    [Header("GrowthStatus")]//캐릭터 성장 능력치. 1레벨 당 해당 value 값만큼 증가.
    public int growHealth;
    public int growAtk;
    public int growDef;

    [Header("Story")]
    public string characterName;//캐릭터 이름
    public string story;//캐릭터 스토리
    public Constants.Belonging belonging;//캐릭터 소속

    [Header("SkillList")]
    public SkillSO skill;
    public PassiveSO passive;

    [Header("Talent")] //특성SO.
    public TalentSO talent_Tier1;
    public TalentSO[] talent_Tier2; //티어 2와 티어 3 특성, 상위 클래스는 여러 개 중 선택해야하므로 '선택 가능한 특성 / 클래스 폭'을 배열로 저장. Character에서 인덱스 값으로 특성 / 클래스를 선택하는 구조.
    public TalentSO[] talent_Tier3;

    [Header("Class")] //클래스SO.
    public ClassSO basicClass;
    public ClassSO[] superiorClass;

    [Header("Equip")] //장비SO
    public WeaponType weaponType;
    public ArmorType armorType;
    public WeaponSO[] weapon; // 캐릭터가 장착 가능한 티어별 무기 SO의 배열. 예시) 인덱스 0 = common 한손검, 1 = rare 한손검, 2 = epic 한손검...
    public ArmorSO[] armor; // 무기와 같은 구조

    [Header("Star")]
    public int basicStar = 1; //기본 성급
}
