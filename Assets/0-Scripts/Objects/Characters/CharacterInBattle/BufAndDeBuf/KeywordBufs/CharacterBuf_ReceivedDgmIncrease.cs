using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_ReceivedDgmIncrease : CharacterBuf //받는 피해 증가 디버프
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.ReceivedDgmIncrease;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "ReceivedDgmIncrease";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        isIndependent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ReducedDmg = ((float)power / 100) * (-1)
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

    public override string GetName()
    {
        BufName = "받는 피해 증가";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 받는 피해가 {power}% 만큼 증가합니다.";

        return base.GetDescription();
    }
}
