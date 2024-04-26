using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterBuf_Corrosion : CharacterBuf
{
    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.Corrosion;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "Corrosion";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, duration, power, stack);
        maxStack = 99;
    }

    public override BonusStat GetAdditionalStat()
    {
        if (Buffer.character.abilityT2 != null && Buffer.character.abilityT2.id == 521) //521. 무장해제 특성 적용 중일 경우
        {
            return new BonusStat
            {
                ExtraDefend = (stack * 0.03f) * (-1) //중첩당 방어력 -0.03%
            };
        }
        else
        {
            return null;
        }
    }


    public override void OnTurnStart()
    {
        base.OnTurnStart();

        int damage = (int)((float)character.health.MaxHealth * 0.02f);

        if (damage < 1)
        {
            damage = 1;
        }


        for (int i = 0; i < stack; i++)
        {
            character.TakeDamageByInt(ref damage, null, BattleKeyWords.AttackDamageType.Buf, Constants.ElementType.Water);
        }
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        DecreaseDuration(1);
    }

    public override string GetName()
    {
        BufName = "부식";

        return base.GetName();
    }

    public override string GetDescription()
    {
        Description = $"{duration}턴 동안, 최대체력의 2%만큼의 피해를 {stack} 중첩만큼 반복해서 입습니다.";

        return base.GetDescription();
    }
}
