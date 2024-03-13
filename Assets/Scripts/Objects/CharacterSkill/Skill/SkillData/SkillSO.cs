using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillScaleType
{
    None,// 대상 하나
    Cross,// 십자형 범위
    Square,// 사각형 범위
    Line,// 직선 범위
    Rhombus// 마름모 범위
}

/*
public enum SkillDamageType
{
    MagicalDamage,
    PhysicalDamage
}
*/

public enum SkillTargetType
{
    Me,
    Enemy,
    Ally
}

[CreateAssetMenu(menuName ="SkillData/SkillData", fileName ="Skill_")]
public class SkillSO : ScriptableObject
{
    [Header("Develope")]
    public string skill_ID;//스킬 식별자

    [Header("Skill_description")]
    public string skillName;
    public string description;

    [Header("Skill_status")]
    public SkillTargetType targetType;//스킬 사용 대상
    //public SkillDamageType damageType;//데미지 타입
    public int cost;//스킬 코스트
    public float coefficient;//스킬 계수
    public float growCoefficient; //레벨 당 증가하는 스킬 계수
    public bool onhit;//스킬이 기본 공격 적중 효과를 발동 시키는 지

    [Header("Skill_Range")]
    public int skillRange;//스킬 사정거리(근거리 공격, 캐릭터 기준 공격이면 1)
    public SkillScaleType scaleType;//스킬 범위 종류(단일 대상 공격, 자신 대상 기술이면 None)
    [Range(0, 10)] public int skillScale;//스킬 범위 칸 수(단일 대상 공격이면 0)

    [Header("Skill_abillity")]
    public string abilityID;//특수 능력
}
