using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Burn : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Burn;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Burn";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);

        this.Buffer = this.character;
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        int damage = (int)((float)character.health.MaxHealth * 0.02f);

        if(damage < 1)
        {
            damage = 1;
        }

        character.TakeDamageByInt(ref damage, null, BattleKeyWords.AttackDamageType.Buf);

        stack -= 1;

        if(stack <= 0)
        {
            DestoyBuf();
        }
    }
}
