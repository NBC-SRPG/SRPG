using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillSO skillData;
    SkillAbilityBase ability;

    public SkillBase(SkillSO skillData)
    {
        this.skillData = skillData;

        Type skillAbillityType = Type.GetType("SkillAbillity_" + skillData.abilityID);

        object obj = Activator.CreateInstance(skillAbillityType);
        ability = obj as SkillAbilityBase;
    }
}
