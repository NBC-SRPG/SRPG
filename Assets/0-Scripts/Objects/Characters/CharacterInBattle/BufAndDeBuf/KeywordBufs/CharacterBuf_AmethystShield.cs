using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_AmethystShield : CharacterBuf //아메가 부여하는 실드 "자수정 방패"
{

    ShieldStat shield_Ame;

    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.AmethystShield;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Positive;

    public override string Keyword { get; protected set; } = "AmethystShield";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, _duration, _power, _stack);
        
        maxStack = 99999999;
        dontDestroy = true;

        shield_Ame = new ShieldStat();
        shield_Ame.Shield = stack;
    }

    public override void OnAddBuf()
    {
        shield_Ame.Shield += stack;

        //(int)(buffer.Defend * (((float)(buffer.character.exSkill.coefficient[0].value) + (buffer.character.Growth.exSkillLevel * buffer.character.exSkill.growthCoefficient)) / 100));

        character.health.AddShield(shield_Ame);

        if (Buffer.character.abilityT3 != null && Buffer.character.abilityT3.id == 831)
        {
            character.curCharacterBufList.debufimmunity = true;
        }
    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {
        if(!character.health.shieldList.Contains(shield_Ame)) 
        {
            DestoyBuf();
        }
    }

    public override void OnTurnStart()
    {
        DecreaseDuration(1);

        if (!character.health.shieldList.Contains(shield_Ame))
        {
            DestoyBuf();
        }
    }

    public override void OnTurnEnd()
    {
        if (!character.health.shieldList.Contains(shield_Ame))
        {
            DestoyBuf();
        }
    }

    public override void OnRoundEnd()
    {
        if (!character.health.shieldList.Contains(shield_Ame))
        {
            DestoyBuf();
        }
    }

    public override void OnDestroy()
    {
        //나중에 또다른 상태이상/디버프 면역 조건이 생길 때 수정이 필요
        if (!character.health.shieldList.Contains(shield_Ame) && Buffer.character.abilityT3 != null && Buffer.character.abilityT3.id == 831)
        {
            character.curCharacterBufList.debufimmunity = false;
        }
        else if (character.health.shieldList.Contains(shield_Ame))
        {
            character.health.RemoveShield(shield_Ame);
        }
    }
}
