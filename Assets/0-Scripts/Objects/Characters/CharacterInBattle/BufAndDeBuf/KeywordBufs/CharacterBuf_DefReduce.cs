using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_DefReduce : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.DefReduce;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "DefReduce";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        isIndependent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraDefend = (float)power / 100 *(-1)
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
