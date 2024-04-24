using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_621: PassiveLogic
{
    //"원거리 사격"
    //공격할 때 대상과 자신의 거리가 멀 수록 데미지가 증가한다  (1칸 당 5%)
    //일반 공격 시작 및 스킬 사용 시, 자신에게 데미지 증가 보너스 스탯 부여. 공격 종료 후 스탯 소멸
    //피유의 2-1 특성.

    BonusStat stat_621 = new BonusStat(); // 보너스 스탯

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        int distance = character.pathFinder.GetManhattenDistance(character.curStandingTile, enemy.curStandingTile);
        stat_621.EnhancedDmg = ((float)coefficient["enhancedDmgRate"] / coefficient["denominator"]) *(distance);  //대상과 자신 사이의 거리를 구하고, 거리 값만큼 5% 데미지 보너스.
        character.tempBonusStat.AddBonusStat(stat_621);
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {

    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_621);
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach (var target in targets) //피유의 Ex스킬은 1인 타겟이므로, 한 번만 순회함.
        {
            int distance = character.pathFinder.GetManhattenDistance(character.curStandingTile, target.curStandingTile);
            stat_621.EnhancedDmg = ((float)coefficient["enhancedDmgRate"] / coefficient["denominator"]) * (distance);  //대상과 자신 사이의 거리를 구하고, 거리 값만큼 5% 데미지 보너스.
            character.tempBonusStat.AddBonusStat(stat_621);
        }
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_621);
    }
}

