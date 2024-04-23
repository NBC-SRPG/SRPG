using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_911: PassiveLogic
{
    //"오파츠 매트릭스"
    //자신의 체력이 30%이하로 떨어질 경우, 2턴간 최대 체력의 30%에 해당하는 실드를 얻는다. 전투 중 1회만 발동한다.
    //에오스의 1 특성

    ShieldStat shield_911;
    bool isSkill_911;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        shield_911 = new ShieldStat();
        isSkill_911 = false;
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        if((float)(character.health.CurHealth) / character.health.TotalHealth <= (float)(coefficient["hpRatio"]) / coefficient["denominator"] && isSkill_911== false)
        {
            shield_911.Shield = (int)(character.health.TotalHealth * ((float)(coefficient["shieldRatio"]) / coefficient["denominator"]));
            shield_911.duration = coefficient["shieldDuration"];
            character.health.AddShield(shield_911);
            isSkill_911= true;
        }
    }
}

