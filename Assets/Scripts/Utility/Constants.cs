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
    public enum Star // 성급 & 한계돌파
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
