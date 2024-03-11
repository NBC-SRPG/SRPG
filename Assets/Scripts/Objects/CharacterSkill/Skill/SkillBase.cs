using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillSO skillData;
    public SkillAbilityBase ability;
    public SkillScaleBase skillScale;

    public CharacterBase character;

    public void Init(CharacterBase character)
    {
        this.character = character;

        ability.init(character);
        InitSkillRange();
    }

    public SkillBase(SkillSO skillData)
    {
        this.skillData = skillData;

        InitSkillAbility();
    }

    private void InitSkillAbility()
    {
        Type skillAbillityType = Type.GetType("SkillAbillity_" + skillData.abilityID);

        object obj = Activator.CreateInstance(skillAbillityType);
        ability = obj as SkillAbilityBase;
    }

    private void InitSkillRange()
    {
        SkillScaleType scaletype = skillData.scaleType;
        int scale = skillData.skillScale;

        switch (scaletype)
        {
            case SkillScaleType.None:
                skillScale = new SkillScale_None(character, scale);
                break;
            case SkillScaleType.Line:
                skillScale = new SkillScale_Line(character, scale);
                break;
            case SkillScaleType.Square:
                skillScale = new SkillScale_Square(character, scale);
                break;
            case SkillScaleType.Cross:
                skillScale = new SkillScale_Cross(character, scale);
                break;
            case SkillScaleType.Rhombus:
                skillScale = new SkillScale_Rhombus(character, scale);
                break;
        }
    }
}
