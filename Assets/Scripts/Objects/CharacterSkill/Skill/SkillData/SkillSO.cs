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
    public string skill_ID;//��ų �ĺ���

    [Header("Skill_description")]
    public string skillName;
    public string description;

    [Header("Skill_status")]
    public SkillTargetType targetType;//��ų ��� ���
    public SkillDamageType damageType;//������ Ÿ��
    public int cost;//��ų �ڽ�Ʈ
    public float coefficient;//��ų ���
    public bool onhit;//��ų�� �⺻ ���� ���� ȿ���� �ߵ� ��Ű�� ��

    [Header("Skill_Range")]
    public int skillRange;//��ų �����Ÿ�(�ٰŸ� ����, ĳ���� ���� �����̸� 1)
    public SkillScaleType scaleType;//��ų ���� ����(���� ��� ����, �ڽ� ��� ����̸� None)
    [Range(0, 10)] public int skillScale;//��ų ���� ĭ ��(���� ��� �����̸� 0)

    [Header("Skill_abillity")]
    public string abilityID;//Ư�� �ɷ�
}
