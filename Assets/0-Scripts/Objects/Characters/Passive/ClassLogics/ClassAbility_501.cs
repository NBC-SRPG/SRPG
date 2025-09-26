using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_501 : PassiveLogic
{
    //집정관
    BonusStat stat_Class501;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class501 = new BonusStat();
        stat_Class501.ExtraHealth = coefficient["increaseHp"];
        stat_Class501.EXCritRate = coefficient["increaseCrtRate"];
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class501);
        character.health.SetHealthSameAsTotal();
    }
}

