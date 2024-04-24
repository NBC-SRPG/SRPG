using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_101 : PassiveLogic
{
    //가디언
    BonusStat stat_Class101;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class101 = new BonusStat();
        stat_Class101.ReducedDmg = (float)(coefficient["reducedDmg"]) / coefficient["denominator"];
        
    }

    public override void OnRoundStart()
    {
        if (!character.tempBonusStat.statList.Contains(stat_Class101))
        {
            character.tempBonusStat.AddBonusStat(stat_Class101);
        }
    }
}

