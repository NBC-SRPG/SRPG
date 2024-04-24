using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Shield : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Shield;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "Shield";

    ShieldStat shield;

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, _duration, _power, _stack);
        isIndependent = true;

        shield = new ShieldStat();
    }

    public override void OnAddBuf()
    {
        base.OnAddBuf();

        shield.Shield = power;
        if (shield.Shield < 1)
        {
            shield.Shield = 1;
        }

        character.health.AddShield(shield);
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        DecreaseDuration(1);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        character.health.RemoveShield(shield);
    }
}
