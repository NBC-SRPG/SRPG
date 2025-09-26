using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterBuf_Stun : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Stun;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Stun";

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

        character.canActing = false;

        character.characterAnim.Animator.SetBool(character.characterAnim.Hit, true);

        turnCnt++;

        Managers.Sound.Play(Sound.EffectBySource, "SE/Battle_CommonSE/Damaged_Stun(Electric)");
    }

    public override void OnRoundEnd()
    {
        base.OnRoundEnd();

        if(turnCnt > 0)
        {
            DecreaseDuration(1);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        character.characterAnim.EndAnimation(character.isWalking);
    }

    public override string GetName()
    {
        BufName = "기절";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 행동할 수 없습니다.";

        return base.GetDescription();
    }
}
