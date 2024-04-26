using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_CrtRateIncrease : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.CrtRateIncrease;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "CrtRateIncrease";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        isIndependent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            EXCritRate = power
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
        BufName = "치명타 확률 증가";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 치명타 확률이 {power}% 만큼 증가합니다.";

        return base.GetDescription();
    }
}
