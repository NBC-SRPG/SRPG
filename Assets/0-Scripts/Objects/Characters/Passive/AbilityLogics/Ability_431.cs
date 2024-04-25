using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_431: PassiveLogic
{
    //"단죄"
    //액티브 스킬의 타겟이 된 적에게 받는 회복 효과가 피해로 전환되는
    //[회복 반전] 상태이상을 부여한다. 1턴간 지속된다.
    //포르테의 3-1 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach(CharacterBase target in targets) //모든 스킬 대상에게 순회
        {
            if (character.CheckEnemy(target)) //적이라면
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.HealReversal, coefficient["bufDuration"], character); //1턴 동안 "치유 반전" 디버프 적용 (힐 받을 때 힐을 체력에 적용하기 전에 힐량에 -1을 곱하는 디버프)
            }
        }
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
    }
}

