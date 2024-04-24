using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_201 : PassiveLogic
{
    //저격수
    BonusStat stat_Class201;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class201 = new BonusStat();
        stat_Class201.EnhancedDmg = (float)(coefficient["increaseDmg_2"]) / coefficient["denominator"];
        stat_Class201.EXCritRate = coefficient["increaseCrtRate_2"];
        character.tempBonusStat.AddBonusStat(stat_Class201);
    }

    private void UpdateStat()
    {
        if (character.health.CurHealth / character.health.TotalHealth >= coefficient["hpRatio"] / coefficient["denominator"])
        {
            stat_Class201.EnhancedDmg = (float)(coefficient["increaseDmg_2"]) / coefficient["denominator"];
            stat_Class201.EXCritRate = coefficient["increaseCrtRate_2"];
        }
        else
        {
            stat_Class201.EnhancedDmg = (float)(coefficient["increaseDmg_1"]) / coefficient["denominator"];
            stat_Class201.EXCritRate = coefficient["increaseCrtRate_1"];
        }
    }

    public override void OnRoundStart()// 
    {
        UpdateStat();
    }

    public override void OnTurnStart()// 턴 시작 시 발동
    {
        UpdateStat();
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        UpdateStat();
    }

    public override void AfterTakeHeal(int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐 받은 이후에
    {
        UpdateStat();
    }
}
}

