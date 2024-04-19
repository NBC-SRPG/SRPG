using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_ReceivedDgmIncrease : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.AtkIncrease;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "AtkIncrease";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraAtk = power
        };
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
}
