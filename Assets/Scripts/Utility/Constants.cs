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
        Stat, //�ܼ��� ���� (ü��/����/���ݷ�, MOV, ġȮ, ġ��, ����, ���ǰ�) ��ġ�� ��½�Ų��.
        Special //���� �Ӹ� �ƴ϶� �ΰ����� Ư�� ��Ŀ������ �߰��ϰų� ���� ��Ų��. 
    }
    public enum BaseClass //�⺻ Ŭ���� 5��.
    {
        Warrior,
        Shooter,
        Assassin,
        Caster,
        Priests
    }
    public enum Star // ���� & �Ѱ赹��
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
