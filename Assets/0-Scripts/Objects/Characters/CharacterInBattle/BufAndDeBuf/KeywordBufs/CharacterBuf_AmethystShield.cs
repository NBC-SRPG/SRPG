using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuf_AmethystShield : CharacterBuf //피유의 패시브 스킬 "표적" 디버프
{
    //이 디버프를 가진 유닛이 적에게 공격을 받을 때, 데미지를 계산하기 전에 적에게 치명타 확률 보너스 스탯을 부여한다. 적의 공격이 끝난 뒤 보너스 스탯은 제거된다.
    ShieldStat shield_Ame;

    public override BattleKeyWords.BufKeyword BufKeyword { get; protected set; } = BattleKeyWords.BufKeyword.AmethystShield;

    public override BattleKeyWords.BufType BufType { get; protected set; } = BattleKeyWords.BufType.Negative;

    public override string Keyword { get; protected set; } = "AmethystShield";

    public override void Init(CharacterBase character, CharacterBase buffer, int _duration, int _power, int _stack)
    {
        base.Init(character, buffer, _duration, _power, _stack);
        shield_Ame.Shield = _stack;
            //(int)(buffer.Defend * (((float)(buffer.character.exSkill.coefficient[0].value) + (buffer.character.Growth.exSkillLevel * buffer.character.exSkill.growthCoefficient)) / 100));

        character.health.AddShield(shield_Ame);

        if(Buffer.character.abilityT3 != null && Buffer.character.abilityT3.id == 831)
        {
            character.curCharacterBufList.debufimmunity = true;
        }
    }

    public override void OnAddBuf()
    {
        shield_Ame.Shield += stack;
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
