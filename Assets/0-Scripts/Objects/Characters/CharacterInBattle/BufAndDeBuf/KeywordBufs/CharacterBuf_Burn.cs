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

        damage = damage * stack;
        character.TakeDamageByInt(ref damage, null, BattleKeyWords.AttackDamageType.Buf, Constants.ElementType.Fire);
        //현재 반복문을 돌려서 데미지를 여러차례 주는게 의미가 없으므로 그냥 stack * damage로 로직을 수정했습니다.
        //코루틴을 돌려서 틱뎀을 구현해보는것도 생각했는데 characterBuf가 monobehavier를 상속을 안해서 일단 보류했습니다.

        /*
        for (int i = 0; i < stack; i++)
        {
            character.TakeDamageByInt(ref damage, null, BattleKeyWords.AttackDamageType.Buf, Constants.ElementType.Fire);
        }
        */

        DecreaseDuration(1);

        Managers.Sound.Play(Sound.EffectBySource, "SE/Battle_CommonSE/Damaged_Burn");
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
