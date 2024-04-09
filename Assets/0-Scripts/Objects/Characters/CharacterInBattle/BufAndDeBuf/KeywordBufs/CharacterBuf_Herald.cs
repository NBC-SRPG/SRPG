using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Herald : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Herald;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Unique";

    ShieldStat shield;

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);

        shield = new ShieldStat();
    }

    public override void OnAddBuf()
    {
        base.OnAddBuf();

        shield.Shield = (int)((float)character.health.MaxHealth * 0.1);

        character.health.AddShield(shield);
    }

    public override BonusStat GetAdditionalStat()
    {
        return new BonusStat
        {
            ExtraDefend = 50,
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

    public override void OnDestroy()
    {
        base.OnDestroy();

        character.health.RemoveShield(shield);
    }
}
