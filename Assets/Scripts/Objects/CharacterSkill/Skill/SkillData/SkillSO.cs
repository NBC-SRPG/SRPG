using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillScaleType
{
    Cross,
    Square,
    Line,
    Rhombus
}

public enum SkillDamageType
{
    MagicalDamage,
    PhysicalDamage
}

[CreateAssetMenu(menuName ="SkillData", fileName ="Skill_")]
public class SkillSO : ScriptableObject
{
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
