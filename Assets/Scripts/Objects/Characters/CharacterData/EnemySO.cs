using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData", fileName = "Enemy_")]
public class EnemySO : CharacterSO
{
    [Header("EnemyType")]
    public bool hasSkill;
    public bool isElite;
}
