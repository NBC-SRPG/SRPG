using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Range,
    Mage
}

public enum CharacterAttribute
{
    Fire,
    Water,
    Grass,
    Bolt,
    Dark,
    Light
}

public enum Belonging
{
    Demon,
    League,
    Order,
    Vegabond
}

[CreateAssetMenu(menuName = "CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public AttackType attackType;//���� Ÿ��
    public CharacterAttribute characterAttribute;//ĳ���� �Ӽ�

    [Header("Develope")]
    public string character_Id;//ĳ���� �ĺ���

    [Header("Status")]//ĳ���� �ɷ�ġ
    public int health;
    public int atk;
    public int def;
    public int res;
    public int mov;
    [Range(0, 10)]public int atk_range;//���� �����Ÿ�(�ٰŸ��� ��� 0����)

    [Header("Story")]
    public string characterName;//ĳ���� �̸�
    public string story;//ĳ���� ���丮
    public Belonging belonging;//ĳ���� �Ҽ�

    [Header("SkillList")]
    public SkillSO skill;
    public PassiveSO passive;
}
