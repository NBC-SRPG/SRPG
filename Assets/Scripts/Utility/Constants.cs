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
        Self,// 자신
        Cross,// 십자형 범위
        Square,// 사각형 범위
        Line,// 직선 범위
        Rhombus// 마름모 범위
    }

    /* //데미지 타입은 기획안에서 더이상 사용하지 않기로 했으므로 일단 주석처리했습니다.
    public enum SkillDamageType
    {
        MagicalDamage,
        PhysicalDamage
    }
    */

    public enum SkillTargetType
    {
        Me, // 대상을 지정할 필요 없음
        Enemy, // 적군 대상
        Ally, // 아군 대상
        All, //피아 미식별
        AllExceptME //피아 미식별 대신 자신은 제외
    }


    public enum PlayerCons
    {
        DefaltLevel = 1,
        DefaltMaxExp = 100,
        MaxLevel = 90,
        DefaltMaxAp = 160
    }


    public enum TalentType
    {
        Stat, //단순히 스탯 (체력/방어력/공격력, MOV, 치확, 치피, 뎀증, 받피감) 수치만 상승시킨다.
        Special //스탯 뿐만 아니라 부가적인 특수 메커니즘을 추가하거나 변경 시킨다. 
    }

    public enum GachaType
    {
        Common,
        PickUp
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

    public enum EquipType
    {
        Weapon,
        Armor
    }

    public enum WeaponType //(무기 종류는 임의로 설정함)
    {
        One_Hand_Blade, //한손 무기 계열
        Two_Hand_Blade, //양손 무기 계열
        Spear, //장병기 계열
        Knuckles, //너클/주먹 계열

        Bow, //활 계열
        Cannon, //대포 계열
        Gun, //총기 계열

        Wand, //완드
        Staff, //지팡이
        Orb //오브
    }

    public enum ArmorType //(방어구 종류는 임의로 설정함)
    {
        Full_Plate_Armor, //전신 갑옷
        Shield, //방패

        Feather_Hat, //깃털 모자
        Chain_Mail, //사슬 갑옷

        Magic_Hat, //마법사 모자
        Robe //로브
    }

}
