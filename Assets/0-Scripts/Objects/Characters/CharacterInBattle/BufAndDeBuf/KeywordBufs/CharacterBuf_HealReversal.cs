using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_HealReversal : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.HealReversal;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "HealReversal";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        cantStack = true;
        base.Init(character, buffer);
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        if (turnCnt > 0)
        {
            DecreaseStack(1);
        }

        turnCnt++;
    }
    public override void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {
        damage *= damage * (-1); //힐량을 데미지로 반전
    }
}
