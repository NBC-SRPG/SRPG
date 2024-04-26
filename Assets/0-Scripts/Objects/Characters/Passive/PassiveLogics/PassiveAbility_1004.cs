using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_1004 : PassiveLogic
{
    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)
    {
        base.OnAttackSuccess(enemy, damage);

        enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Bleed, 0, character, 20, 10);
    }
}
