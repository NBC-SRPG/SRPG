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

        maxStack = 99999999;
        dontDestroy = true;

        shield = new ShieldStat();
        shield.Shield = stack;
    }

    public override void OnAddBuf()
    {
        base.OnAddBuf();
        shield.Shield += stack;
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

        if (!character.health.shieldList.Contains(shield))
        {
            DestoyBuf();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        character.health.RemoveShield(shield);
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        if (!character.health.shieldList.Contains(shield))
        {
            DestoyBuf();
        }
    }

    public override void OnTurnEnd()
    {
        if (!character.health.shieldList.Contains(shield))
        {
            DestoyBuf();
        }
    }

    public override void OnRoundEnd()
    {
        if (!character.health.shieldList.Contains(shield))
        {
            DestoyBuf();
        }
    }

    public override string GetName()
    {
        BufName = "보호막";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, {stack} 만큼의 피해를 흡수합니다.";

        return base.GetDescription();
    }
}
