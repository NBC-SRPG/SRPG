using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillData/PassiveData", fileName = "Passive_")]
public class PassiveSO : ReflectionableSO
{
    [Header("Develope")]
    public int id;
    
    [Header("Description")]
    public string passiveName;
    public string description;

}