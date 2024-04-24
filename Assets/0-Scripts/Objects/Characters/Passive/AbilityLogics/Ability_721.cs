using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_721: PassiveLogic
{
    //"자기장 배리어"
    //Ex 스킬을 시전한 뒤,가한 데미지의 40%에 해당하는 보호막을 얻는다. 1턴 간 유지된다.
    //시스의 2-1 특성

    ShieldStat shield_721;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        shield_721 = new ShieldStat();
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        shield_721.Shield = (int)(damage.damage * ((float)(coefficient["shieldRatio"]) / coefficient["denominator"]));
        shield_721.duration = coefficient["shieldDuration"];
        character.health.AddShield(shield_721);
    }

    public override void OnTurnStart() 
    {
    }
}

