using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_105 : PassiveLogic
{
    //ID 005. 레인 Rain
    //패시브 스킬
    //레인의 일반 공격은 적에게 2턴동안 [부식] 상태이상을 부여한다.  ( 부식 :  매 턴이 시작 될 때마다 중첩당 최대체력의 2% 데미지를 입는다. 최대 중첩 수 : 99)
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnRoundStart()// 
    {

    }

    public override void OnTurnStart()// 턴 시작 시 발동
    {

    }

    public override void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {

    }

    public override void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public override void OnPassEnemy(CharacterBase enemtCharacter)// 적군 위를 지나갔을 때 발동
    {

    }

    public override void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {

    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {

    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, 0, 1); //2턴 동안 1스택의 부식 디버프 적용
    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, 0, 1); //2턴 동안 3스택의 부식 디버프 적용. 3회 타격하므로 총합 3스택.
    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public override void AfterTakeAttacked(CharacterBase enemy)// 공격 받은 이후에
    {

    }

    public override void OnTakeAttack(CharacterBase enemy)// 공격 받기 이전에
    {

    }

    public override void OnTakeDamage(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 입을 때
    {

    }

    public override void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {

    }

    public override void AfterTakeDamage(int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 데미지를 받은 이후에
    {

    }

    public override void AfterTakeHeal(int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐 받은 이후에
    {

    }

    public override void OnStartMoving()// 이동 시
    {

    }

    public override void OnEndMoving()// 이동 끝난 직후
    {

    }

    public override void OnEndActing()// 행동이 끝난 뒤
    {

    }

    public override void OnRoundEnd()
    {

    }

    public override void OnTurnEnd()// 턴이 끝날 때
    {

    }

    public override void OnKillEnemy(CharacterBase enemy, Constants.ElementType characterAttribute = Constants.ElementType.None)// 적 처치 시
    {

    }

    public override void OnDieInBattle(CharacterBase killer)// 전투 중 사망 시
    {

    }

    public override void OnDie()// 사망 시
    {

    }

    public override void OnUpdate()// 실시간 판정
    {

    }
}

