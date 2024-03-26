using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillSO skillData;
    public SkillAbilityBase skillAbility;
    public SkillScaleBase skillScaleClass;

    public CharacterBase character;

    public List<OverlayTile> targetTiles;

    //스킬 시전자 설정
    public void Init(CharacterBase character)
    {
        this.character = character;

        skillAbility.init(character);
        InitSkillRange();
    }

    //스킬 생성자
    public SkillBase(SkillSO skillData)
    {
        this.skillData = skillData;

        InitSkillAbility();
    }

    //스킬 특수 능력 생성자
    private void InitSkillAbility()
    {
        Type skillAbillityType = Type.GetType("SkillAbility_" + skillData.abilityID);

        object obj = Activator.CreateInstance(skillAbillityType);
        skillAbility = obj as SkillAbilityBase;
    }

    private void InitSkillRange()//스킬 범위 생성자
    {
        Constants.SkillScaleType scaletype = skillData.scaleType;
        int scale = skillData.skillScale;

        switch (scaletype)
        {
            case Constants.SkillScaleType.Self:
            case Constants.SkillScaleType.None:
                skillScaleClass = new SkillScale_None(character, scale);
                break;
            case Constants.SkillScaleType.Line:
                skillScaleClass = new SkillScale_Line(character, scale);
                break;
            case Constants.SkillScaleType.Square:
                skillScaleClass = new SkillScale_Square(character, scale);
                break;
            case Constants.SkillScaleType.Cross:
                skillScaleClass = new SkillScale_Cross(character, scale);
                break;
            case Constants.SkillScaleType.Rhombus:
                skillScaleClass = new SkillScale_Rhombus(character, scale);
                break;
        }
    }
}

