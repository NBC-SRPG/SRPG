using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_Bind : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Bind;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Bind";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);

        onlyOne = true;
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
            DecreaseDuration(1);
        }
    }

    public override string GetName()
    {
        BufName = "속박";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 캐릭터의 이동거리가 0이 됩니다.";

        return base.GetDescription();
    }
}
