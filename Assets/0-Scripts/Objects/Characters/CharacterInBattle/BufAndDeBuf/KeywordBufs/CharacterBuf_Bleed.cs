using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleKeyWords;

public class CharacterBuf_Bleed : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Bleed;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Bleed";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        maxStack = 99;
        this.Buffer = this.character;
    }

    public override void OnEndActing()
    {
        base.OnEndActing();

        for (int i = 0; i < stack; i++) //출혈에 걸린 상태에서 움직일 시, 피해량 power의 데미지를 중첩된 출혈 수 만큼 반복.
        {
            character.TakeDamageByInt(ref power, null, BattleKeyWords.AttackDamageType.Buf);
        }

        stack = stack/2; //그리고 스택이 절반으로 줄어든다.
        if (stack == 0)
        {
            DestoyBuf(); //스택이 0이 되면 출혈 제거.
        }
    }

    public override string GetName()
    {
        BufName = "출혈";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"행동 한 이후에 {power}의 피해를 {stack} 중첩만큼 반복해서 입습니다.\r\n 피해를 입은 이후 중첩이 절반으로 감소합니다.";

        return base.GetDescription();
    }

}
