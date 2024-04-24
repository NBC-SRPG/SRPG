using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_302 : PassiveLogic
{
    //전투광
    BonusStat stat_Class302;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class302 = new BonusStat();
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class302);
    }

    public override void OnKillEnemy(CharacterBase enemy, Constants.ElementType characterAttribute = Constants.ElementType.None)// 적 처치 시
    {
        stat_Class302.ExtraAtk += (float)(coefficient["increaseAtk"]) / coefficient["denominator"];
    }
}

