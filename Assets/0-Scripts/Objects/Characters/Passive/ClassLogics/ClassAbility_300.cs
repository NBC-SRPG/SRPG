using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_300 : PassiveLogic
{
    //암살자
    BonusStat stat_Class300;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class300 = new BonusStat();
        stat_Class300.EXCritRate = coefficient["increaseCrtRate"];
        stat_Class300.EXCritDMG = coefficient["increaseCrtDmg"];
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class300);
    }
}

