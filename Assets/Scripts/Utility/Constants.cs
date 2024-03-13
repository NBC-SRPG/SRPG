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
    public enum CharacterCons 
    {
        defaltCtr = 10, //기본 치확. 10%
        defaltCtd = 120, //기본 치피. 통상의 1.2배 데미지.
    }
}
