using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillData/PassiveData", fileName = "PassiveSkillSO_")]
public class PassiveSkillSO : PassiveSO
{
    [Header("Develope")]
    public int id;
    
    [Header("Description")]
    public string passiveName;
    [TextArea]
    public string description;
    public Sprite icon;

}