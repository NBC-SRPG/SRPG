using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterBuf_Burn : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Burn;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Burn";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        maxStack = 2; //화상은 최대 2중첩까지만 가능
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        int damage = (int)((float)character.health.MaxHealth * 0.04f); //이 버프를 가진 캐릭터의 최대 체력의 4% 데미지

        if(damage < 1)
        {
            damage = 1;
        }

        for (int i = 0; i < stack; i++)
        {
            character.TakeDamageByInt(ref damage, null, BattleKeyWords.AttackDamageType.Buf, Constants.ElementType.Fire);
        }

        DecreaseDuration(1);
    }

    public override string GetName()
    {
        BufName = "화상";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 최대 체력의 4%의 해당하는 피해를 {stack} 중첩 만큼 반복해서 입습니다.";

        return base.GetDescription();
    }
}
