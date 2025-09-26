using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_811: PassiveLogic
{
    //"흡혈귀"
    //공격 시, 입힌 피해의 30%만큼 회복한다.
    //아메의 1 특성
    List<CharacterBase> characterSelf;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        characterSelf = new List<CharacterBase>() { character };
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        int healAmount = (int)(damage.damage * ((float)(coefficient["drainRate"]) / coefficient["denominator"]));
        BattleManager.Instance.ExtraSkillHeal(character, healAmount, characterSelf, BattleKeyWords.AttackDamageType.Passive);
    }
}

