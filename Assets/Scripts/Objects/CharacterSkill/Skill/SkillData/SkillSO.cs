using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SkillData/SkillData", fileName ="Skill_")]
public class SkillSO : ScriptableObject
{
    [Header("Develope")]
    public int skill_ID;//스킬 식별자

    [Header("Skill_description")]
    public string skillName;
    public string description;

    [Header("Skill_status")]
    public Constants.SkillTargetType targetType;//스킬 사용 대상
    //public Constants.SkillDamageType damageType;//데미지 타입 //데미지 타입은 기획안에서 더이상 사용하지 않기로 했으므로 일단 주석처리했습니다.
    public int cost;//스킬 코스트
    public float coefficient;//스킬 계수
    public float growthCoefficient; //레벨 당 증가하는 스킬 계수
    public bool onhit;//스킬이 기본 공격 적중 효과를 발동 시키는 지

    [Header("Skill_Range")]
    public int skillRange;//스킬 사정거리(근거리 공격, 캐릭터 기준 공격이면 1)
    public Constants.SkillScaleType scaleType;//스킬 범위 종류(단일 대상 공격, 자신 대상 기술이면 None)
    [Range(0, 10)] public int skillScale;//스킬 범위 칸 수(단일 대상 공격이면 0)
    
    [Header("Skill_abillity")]
    public int abilityID;//특수 능력
}
