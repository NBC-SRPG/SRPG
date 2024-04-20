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
            if (target.CheckEnemy(target)) //적이라면
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.HealReversal, 1, character); //1턴 동안 "치유 반전" 디버프 적용 (힐 받을 때 힐량에 -1을 곱하는 디버프)
            }
        }
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

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

