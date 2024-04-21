using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_ReceivedDgmReduce : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.ReceivedDgmReduce;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "ReceivedDgmReduce";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        isIndependent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ReducedDmg = (power / 100)
        };
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        if (turnCnt > 0)
        {
            DecreaseDuration(1);
        }

        turnCnt++;
    }

}
