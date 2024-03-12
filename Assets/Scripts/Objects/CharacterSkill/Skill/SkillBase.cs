using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillSO skillData;
    public SkillAbilityBase skillAbility;
    public SkillScaleBase skillScale;

    public CharacterBase character;

    //스킬 시전자 설정
    public void Init(CharacterBase character)
    {
        this.character = character;

        skillAbility.init(character);
        InitSkillRange();
    }

    public SkillBase(SkillSO skillData)
    {
        this.skillData = skillData;

        InitSkillAbility();
    }

    //스킬 특수 능력 생성자
    private void InitSkillAbility()
    {
        Type skillAbillityType = Type.GetType("SkillAbillity_" + skillData.abilityID);

        object obj = Activator.CreateInstance(skillAbillityType);
        skillAbility = obj as SkillAbilityBase;
    }

    private void InitSkillRange()//스킬 범위 생성자
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
