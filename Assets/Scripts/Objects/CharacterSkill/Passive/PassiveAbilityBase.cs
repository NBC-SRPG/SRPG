using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbilityBase
{
    protected CharacterBase character;

    public virtual void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public virtual void OnStartTurn()// 턴 시작 시 발동
    {

    }

    public virtual void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {

    }

    public virtual void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public virtual void OnPassEnemy(CharacterBase enemtCharacter)// 적군 위를 지나갔을 때 발동
    {

    }

    public virtual void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public virtual void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {

    }

    public virtual void OnAttackSuccess(CharacterBase enemy, int damage)// 공격 적중 시
    {

    }

    public virtual void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {

    }

    public virtual void OnTakeAttacked(CharacterBase enemy)// 공격 타겟이 되었을 때
    {

    }

    public virtual void OnTakeDamage(CharacterBase enemy)// 공격 받았을 때
    {

    }

    public virtual void OnStartMoving()// 이동 시
    {

    }

    public virtual void OnEndMoving()// 이동 끝난 직후
    {

    }

    public virtual void OnEndActing()// 행동이 끝난 뒤
    {

    }

    public virtual void OnEndTurn()// 턴이 끝날 때
    {

    }

    public virtual void OnDieInBattle(CharacterBase killer)// 전투 중 사망 시
    {

    }

    public virtual void OnDie()// 사망 시
    {

    }
}

public class PassiveAbility_ : PassiveAbilityBase // 테스트용
{
    public override void OnEnemyPassesMe(CharacterBase enemy)
    {
        base.OnEnemyPassesMe(enemy);
        enemy.BlockMoving();//ZOC 테스트 

        AnimationController.instance.EnqueuedefendAnimation(enemy, character);
    }

    public override void OnTakeAttacked(CharacterBase enemy)// 반격 테스트
    {
        base.OnTakeAttacked(enemy);

        if (!character.isDead)
        {
            character.CounterAttack(enemy);
        }
    }
}