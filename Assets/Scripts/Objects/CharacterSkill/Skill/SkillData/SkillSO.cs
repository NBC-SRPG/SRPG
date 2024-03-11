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

public enum SkillDamageType
{
    MagicalDamage,
    PhysicalDamage
}

[CreateAssetMenu(menuName ="SkillData/SkillData", fileName ="Skill_")]
public class SkillSO : ScriptableObject
{
    [Header("Develope")]
    public string skill_ID;

    [Header("Skill_description")]
    public string skillName;
    public string description;

    [Header("Skill_status")]
    public SkillDamageType damageType;
    public int cost;
    public float coefficient;
    public int skillRange;//스킬 사정거리
    public SkillScaleType scaleType;
    [Range(0, 2)] public int skillScale;

    [Header("Skill_abillity")]
    public string abilityID;
}
