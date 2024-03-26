using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Burn : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get => base.BufKeyword; protected set => base.BufKeyword = BattleKeyWords.BufKeyword.Burn; }
    public override BattleKeyWords.BufType BufType { get => base.BufType; protected set => base.BufType = BattleKeyWords.BufType.Negative; }

    public override string Keyword { get => base.Keyword; protected set => base.Keyword = "Burn"; }

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);

        this.Buffer = this.character;
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        character.health.TakeDamage(stack);

        stack /= 2;

        if(stack <= 0)
        {
            DestoyBuf();
        }
    }
}
