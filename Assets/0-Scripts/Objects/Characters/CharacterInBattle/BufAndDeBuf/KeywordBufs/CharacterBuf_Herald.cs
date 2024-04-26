using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Herald : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Herald;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Unique";

    ShieldStat shield;

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);

        shield = new ShieldStat();
    }

    public override void OnAddBuf()
    {
        base.OnAddBuf();

        shield.Shield = (int)((float)character.health.MaxHealth * 0.1);

        if(shield.Shield < 1)
        {
            shield.Shield = 1;
        }

        character.health.AddShield(shield);
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraDefend = 0.5f,
            ExtraMov = 2
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

    public override void OnDestroy()
    {
        base.OnDestroy();

        character.health.RemoveShield(shield);
    }
}
