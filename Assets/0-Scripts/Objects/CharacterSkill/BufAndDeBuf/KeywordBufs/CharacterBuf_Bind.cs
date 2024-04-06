using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Bind : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Bind;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Bind";

    public override void Init(CharacterBase character, CharacterBase buffer)
    {
        base.Init(character, buffer);

        cantStack = true;
    }

    public override void OnAddBuf()
    {
        base.OnAddBuf();

        character.BlockMoving();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        character.leftWalkRange = 0;

        turnCnt++;
    }

    public override void OnRoundEnd()
    {
        base.OnRoundEnd();

        if (turnCnt > 0)
        {
            DecreaseStack(1);
        }
    }
}
