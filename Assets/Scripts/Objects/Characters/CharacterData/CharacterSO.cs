using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Range,
    Mage
}

[CreateAssetMenu(menuName = "CharacterData", fileName ="Character_")]
public class CharacterSO : ScriptableObject
{
    public AttackType attackType;

    [Header("Status")]
    public int health;
    public int atk;
    public int def;
    public int res;
    public int agi;

    [Header("Story")]
    public string story;

    //[Header("SkillList")]

}
