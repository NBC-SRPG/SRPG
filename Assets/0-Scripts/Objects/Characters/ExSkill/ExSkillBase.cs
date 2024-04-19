using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExSkillBase
{
    public ExSkillSO skillData;
    public ExSkillLogic skillAbility;
    public SkillScaleBase skillScaleClass;

    public CharacterBase character;

    public List<OverlayTile> targetTiles;

    public int SkillFigure_ATK
    {
        get { return (int)(skillData.coefficient * character.character.atk); }
    }

    //체력 계수 스킬에 필요해서 이 필드를 추가했습니다.
    public int SkillFigure_HP
    {
        get { return (int)(skillData.coefficient * character.character.hp); }
    }

    //위와 같음
    public int SkillFigure_DEF
    {
        get { return (int)(skillData.coefficient * character.character.def); }
    }


    //스킬 시전자 설정
    public void Init(CharacterBase character)
    {
        this.character = character;

        if (skillAbility != null)
        {
            skillAbility.init(character);
        }

        InitSkillRange();
    }

    //스킬 생성자
    public ExSkillBase(ExSkillSO skillData)
    {
        this.skillData = skillData;

        InitSkillAbility();
    }

    public ExSkillBase()
    {

    }

    //스킬 특수 능력 생성자
    private void InitSkillAbility()
    {
        Type skillAbillityType = Type.GetType("SkillAbility_" + skillData.abilityID);

        if (skillAbillityType == null)
        {
            return;
        }

        object obj = Activator.CreateInstance(skillAbillityType);
        skillAbility = obj as ExSkillLogic;
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
            case Constants.SkillScaleType.Moon:
                skillScaleClass = new SkillScale_Moon(character, scale);
                break;
        }
    }
}

