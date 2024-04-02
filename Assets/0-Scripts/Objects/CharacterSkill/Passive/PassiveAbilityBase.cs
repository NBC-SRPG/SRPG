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

    public virtual void OnRoundStart()// 
    {

    }

    public virtual void OnTurnStart()// 턴 시작 시 발동
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

    public virtual void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 적중 시
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
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

    public virtual void OnRoundEnd()
    {

    }

    public virtual void OnTurnEnd()// 턴이 끝날 때
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

        AnimationController.instance.EnqueuedefendAnimation(enemy, character);
        character.CounterAttack(enemy);// 이동 방해중에 반격 테스트(BlockMoving 함수에 애니메이션 추가 코드가 들어있어 움직임을 막기 전에 먼저 반격해야됨)

        enemy.BlockMoving();//ZOC 테스트 
    }

    //public override void OnTakeAttacked(CharacterBase enemy)// 반격 테스트
    //{
    //    base.OnTakeAttacked(enemy);

    //    if (!character.isDead)
    //    {
    //        character.CounterAttack(enemy);
    //    }
    //}

    //public override void OnAttackSuccess(CharacterBase enemy, int damage)
    //{
    //    base.OnAttackSuccess(enemy, damage);

    //    enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2);// 상태이상 화상 테스트
    //    enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Bleed, 10);// 상태이상 출혈 테스트
    //}

    public override void OnPassAlly(CharacterBase allyCharacter)// 체력 회복 테스트
    {
        base.OnPassAlly(allyCharacter);

        allyCharacter.health.HealHealth(10);
    }
}