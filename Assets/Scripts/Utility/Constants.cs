using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public enum Sound
    {
        Bgm,
        Effect,
        Max,
    }

    public enum AttackType
    {
        Melee,
        Range,
        //Mage
    }

    public enum CharacterAttribute
    {
        Fire,
        Water,
        Grass,
        Bolt,
        Dark,
        Light
    }

    public enum Belonging
    {
        Demon,
        League,
        Order,
        Vegabond
    }

    public enum SkillScaleType
    {
        None,// 대상 하나
        Cross,// 십자형 범위
        Square,// 사각형 범위
        Line,// 직선 범위
        Rhombus// 마름모 범위
    }

    public enum SkillDamageType
    {
        MagicalDamage,
        PhysicalDamage
    }

    public enum SkillTargetType
    {
        Me,
        Enemy,
        Ally
    }


    public enum PlayerCons
    {
        DefaltLevel = 1,
        DefaltMaxExp = 100,
        MaxLevel = 90,
        DefaltMaxAp = 160
    }


    public enum TraitType
    {
        Stat, //단순히 스탯 (체력/방어력/공격력, MOV, 치확, 치피, 뎀증, 받피감) 수치만 상승시킨다.
        Special //스탯 뿐만 아니라 부가적인 특수 메커니즘을 추가하거나 변경 시킨다. 
    }
    public enum BaseClass //기본 클래스 5종.
    {
        Warrior,
        Shooter,
        Assassin,
        Caster,
        Priests
    }

    public enum Star // 성급 & 한계돌파. (정수값 = 해당 성급의 최대 레벨)
    {
        Star_1 = 30,
        Star_2 = 40,
        Star_3 = 50,
        Star_4 = 60,
        Star_5 = 70,
        LimitBreak_1 = 75,
        LimitBreak_2 = 80,
        LimitBreak_3 = 85,
        LimitBreak_4 = 90
    }

}
