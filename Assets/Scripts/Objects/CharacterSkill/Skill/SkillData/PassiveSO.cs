using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillData/PassiveData", fileName = "Passive_")]
public class PassiveSO : ScriptableObject
{
    [Header("Description")]
    public string PassiveName;
    public string description;

    [Header("Develope")]
    public string passive_Id;
}
