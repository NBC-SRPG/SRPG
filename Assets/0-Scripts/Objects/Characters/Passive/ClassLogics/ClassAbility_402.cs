using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_402 : PassiveLogic
{
    //대마법사
    BonusStat stat_Class402;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class402 = new BonusStat();
        stat_Class402.EnhancedDmg = (float)(coefficient["increaseDmg"]) / coefficient["denominator"];
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class402);
    }

}

