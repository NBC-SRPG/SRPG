using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_532: PassiveLogic
{
    //"허리케인"
    //Ex스킬의 비용이 10  증가하는 대신,
    //데미지가 50% 증가한다.
    //레인의 3-2 특성.
    BonusStat stat_532 = new BonusStat(); // 보너스 스탯
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

    }

    public override void OnStageStart()
    {
        character.skillCost = character.curCharacterSkill.skillData.cost;
        character.skillCost += coefficient["addCost"];
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        stat_532.EnhancedDmg = (float)(coefficient["enhanceDmgRate"]) / (float)(coefficient["denominator"]); //스킬 사용 ~ 스킬 사용 종료까지 주는 데미지가 50% 상승한다.
        character.tempBonusStat.AddBonusStat(stat_532);
    }

    public override void OnEndSkill(List<CharacterBase> target)
    {
        character.tempBonusStat.RemoveBonusStat(stat_532);
    }
}

