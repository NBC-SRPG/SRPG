using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_100 : PassiveLogic
{
    //전사
    BonusStat stat_Class100;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class100 = new BonusStat();
        stat_Class100.ExtraAtk = (float)(coefficient["increaseAtk"]) / coefficient["denominator"];
        stat_Class100.ReducedDmg = (float)(coefficient["reducedDmg"]) / coefficient["denominator"];
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class100);
    }
}

