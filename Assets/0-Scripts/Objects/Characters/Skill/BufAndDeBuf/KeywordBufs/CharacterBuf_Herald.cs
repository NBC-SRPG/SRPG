using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Herald : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Herald;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Unique";

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraDefend = character.character.def / 2,
            ExtraMov = 2
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
