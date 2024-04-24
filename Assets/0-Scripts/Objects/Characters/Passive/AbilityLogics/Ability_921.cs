using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_921: PassiveLogic
{
    //"결투가"
    //각 행동마다 첫번째 공격의 데미지가 30% 증가한다.
    //에오스의 2-1 특성

    BonusStat stat_921;


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_921 = new BonusStat();
        stat_921.EnhancedDmg = (float)(coefficient["enhancedDmgRate"]) / coefficient["denominator"];
    }



    public override void OnRoundStart()// 
    {

    }

    public override void OnTurnStart()// 턴 시작 시 발동
    {
        character.tempBonusStat.AddBonusStat(stat_921);
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {

    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_921);
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_921);
    }



    public override void OnEndActing()// 행동이 끝난 뒤
    {
        character.tempBonusStat.AddBonusStat(stat_921);
    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {
        character.tempBonusStat.RemoveBonusStat(stat_921);
    }

}

