using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_500 : PassiveLogic
{
    //성직자
    BonusStat stat_Class500;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class500 = new BonusStat();
        stat_Class500.ExtraHealth = coefficient["increaseHp"];
        character.tempBonusStat.AddBonusStat(stat_Class500);
    }

    public override void OnTurnStart()// 턴 시작 시 발동
    {
        character.player.GainMana(coefficient["gainMana"]);
    }

}

