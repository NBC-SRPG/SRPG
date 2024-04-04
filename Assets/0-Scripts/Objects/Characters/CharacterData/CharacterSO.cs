using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "CharacterData/CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public AttackMethod attackMethod;     //공격 타입
    public ElementType elementType;      //캐릭터 속성
    
    [Header("Develope")]
    public int id;      //캐릭터 식별자

    [Header("Status")]      //캐릭터 능력치
    public int hp;
    public int atk;
    public int def;
    public int mov;
    [Range(0, 10)]public int range;     //공격 사정거리(근거리의 경우 0으로)

    [Header("StatusPerLevel")]      //캐릭터 성장 능력치. 1레벨 당 해당 value 값만큼 증가.
    public int hpPerLv;
    public int atkPerLv;
    public int defPerLv;

    [Header("Story")]
    public string characterName;        //캐릭터 이름
    public string story;        //캐릭터 스토리
    public Faction faction;     //캐릭터 소속

    /*
    [Header("Skill")]
    public int ExSkill;
    public int PassiveSkill;
    */
    
    [Header("Ability")]      //특성SO.
    public int abilityT1;
    public int[] abilityT2; //티어 2와 티어 3 특성, 상위 클래스는 여러 개 중 선택해야하므로 '선택 가능한 특성 / 클래스 폭'을 배열로 저장.
    public int[] abilityT3;

    [Header("Class")]       //클래스SO.
    public int basicClass;
    public int[] superiorClass;

    // TODO: 장비리팩터링시 처리예정
    /*
    [Header("Equip")] //장비SO
    public WeaponType weaponType;
    public ArmorType armorType;
    public WeaponSO[] weapon; // 캐릭터가 장착 가능한 티어별 무기 SO의 배열. 예시) 인덱스 0 = common 한손검, 1 = rare 한손검, 2 = epic 한손검...
    public ArmorSO[] armor; // 무기와 같은 구조
    */

    [Header("Star")]
    public int basicStar; //기본 성급

    [Header("Animator")]
    public string animatorName;
}
