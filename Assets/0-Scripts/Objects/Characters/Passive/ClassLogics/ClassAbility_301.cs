using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_301 : PassiveLogic
{
    //처형인
    BonusStat stat_Class301;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class301 = new BonusStat();
        stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
        stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
        character.tempBonusStat.AddBonusStat(stat_Class301);
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        if (enemy.health.CurHealth / enemy.health.TotalHealth < coefficient["hpRatio"] / coefficient["denominator"])
        {
            stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_2"]) / coefficient["denominator"];
            stat_Class301.EXCritRate = coefficient["increaseCrtRate_2"];
        }
        else
        {
            stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
            stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
        }
    }


    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        if (targets[0].health.CurHealth / targets[0].health.TotalHealth < coefficient["hpRatio"] / coefficient["denominator"])
        {
            stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_2"]) / coefficient["denominator"];
            stat_Class301.EXCritRate = coefficient["increaseCrtRate_2"];
        }
        else
        {
            stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
            stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
        }
    }


    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
        stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
    }

    public override void OnEndAttack(CharacterBase enemy)
    {
        stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
        stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
    }


    public override void OnEndActing()// 행동이 끝난 뒤
    {
        stat_Class301.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
        stat_Class301.EXCritRate = coefficient["increaseCrtRate_1"];
    }

}

