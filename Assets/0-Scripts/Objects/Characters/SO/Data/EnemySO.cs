using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "CharacterData/EnemyData", fileName = "Enemy_")]
public class EnemySO
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
    public int range;     //공격 사정거리(근거리의 경우 0으로)
    public Faction faction;     //캐릭터 소속

    public ExSkillSO exSkillSO;
    public PassiveSkillSO passiveSkillSO;

    [Header("Ability")]      //특성SO.
    public AbilitySO abilityT1;
    public AbilitySO abilityT2;
    public AbilitySO abilityT3;

    [Header("Class")]       //클래스SO.
    public ClassSO basicClass;
    public ClassSO superiorClass;


    [Header("EquipList")]
    public EquipSO weapon;
    public EquipSO armor;

    [Header("Animator")]
    public string animatorName;

    [Header("EnemyType")]
    public bool hasSkill;
    public bool isElite;
}
