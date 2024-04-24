using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_502 : PassiveLogic
{
    //치유사
    BonusStat stat_Class502;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class502 = new BonusStat();
        stat_Class502.ExtraHealth = coefficient["increaseHp"];
       
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class502);
    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        heal.damage = heal.damage * (int)(1 + ((float)(coefficient["increaseHealRate"]) / coefficient["denominator"]));
    }
}

