using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillScaleType
{
    None,// ��� �ϳ�
    Cross,// ������ ����
    Square,// �簢�� ����
    Line,// ���� ����
    Rhombus// ������ ����
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
    public int skillRange;//��ų �����Ÿ�
    public SkillScaleType scaleType;
    [Range(0, 2)] public int skillScale;

    [Header("Skill_abillity")]
    public string abilityID;
}
