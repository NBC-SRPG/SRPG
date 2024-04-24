using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_400 : PassiveLogic
{
    //마법사
    BonusStat stat_Class400;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class400 = new BonusStat();
        stat_Class400.EnhancedDmg = (float)(coefficient["increaseDmg"]) / coefficient["denominator"];
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class400);
    }
}

