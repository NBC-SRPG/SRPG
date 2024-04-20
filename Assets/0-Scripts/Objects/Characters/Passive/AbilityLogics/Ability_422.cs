using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_422: PassiveLogic
{
    //"은총"
    //포르테의 치유를 받은 아군은 1턴 동안 치명타 확률이 10% 증가한다.
    //포르테의 2-2 특성.

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

    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        if (!target.CheckEnemy(target)) //적이 아닐 경우
        {
            if (character.character.abilityT3.id == coefficient["abilityIdT3"]) //현재 432번 특성 "신의 축복"이 적용 중일 경우
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.CrtRateIncrease, coefficient["bufDuration_2"], character, coefficient["bufCoefficients_2"]); //아군일 경우 2턴 간 치명타 확률 15%증가
            }
            else 
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.CrtRateIncrease, coefficient["bufDuration"], character, coefficient["bufCoefficients"]); //아군일 경우 1턴 간 치명타 확률 10%증가
            }
        }
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

