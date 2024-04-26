using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_AtkIncrease : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.AtkIncrease;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "AtkIncrease";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        isIndependent = true;
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraAtk = (float)power / 100
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
        BufName = "공격력 증가";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 공격력이 {power}% 상승 합니다.";

        return base.GetDescription();
    }

}
