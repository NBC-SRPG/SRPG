using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_932: PassiveLogic
{
    //"재기동"
    //액티브 스킬로 적을 처치했을 시, 다시 행동할 수 있다.
    //에오스의 3-1 특성

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }


    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        if (target[0].isDead)
        {
            character.canActing = true;
            character.didWalk = false;
            character.didAttack = false;
            character.canMoveSkil = true;
            character.ActivateSkill();
        }
    }


    public override void OnEndActing()// 행동이 끝난 뒤
    {

    }

    public override void OnKillEnemy(CharacterBase enemy, Constants.ElementType characterAttribute = Constants.ElementType.None)// 적 처치 시
    {

    }


    public override void OnUpdate()// 실시간 판정
    {

    }
}

