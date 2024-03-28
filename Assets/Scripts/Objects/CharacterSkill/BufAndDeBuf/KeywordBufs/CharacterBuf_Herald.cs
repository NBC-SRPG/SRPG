using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Herald : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Test_UniqBuf;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Unique";

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraDefend = character.character.Defence / 2,
            ExtraMov = 2
        };
    }

    public override void OnRoundStart()
    {
        base.OnRoundStart();

        stack -= 1;
        Debug.Log("left stack " + stack);

        if (stack <= 0)
        {
            Debug.Log("destroy");
            DestoyBuf();
        }
    }
}
