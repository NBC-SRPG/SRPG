using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_ReceivedDgmReduce : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.ReceivedDgmReduce;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "ReceivedDgmReduce";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ReducedDmg = power
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
