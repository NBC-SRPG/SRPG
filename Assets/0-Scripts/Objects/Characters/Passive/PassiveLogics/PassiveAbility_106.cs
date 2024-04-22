using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_106 : PassiveLogic
{
    //ID 006. 피유 Piyu
    //패시브 스킬
    //피유의 일반 공격 및 스킬 공격은 적에게 1턴동안 [표적] 상태이상을 부여한다. ( 표적 : 공격을 받을 때 치명타가 발생할 확률이  20%  증가한다.) 
    //세부 설명: 일반 공격은 공격 적중 후에 디버프를 걸지만, 스킬 공격은 스킬을 사용하기 이전에 디버프를 걸도록 해서 스킬 공격의 메리트를 조금 더 높였습니다.

    bool IsabilityT3_632; //특성 632번 "끝없는 추적"이 적용 중이면 표적을 걸 때 지속 턴 +!

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        if(character.character.abilityT3.id == 632)
        {
            IsabilityT3_632 = true;
        }
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
        if (IsabilityT3_632)
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration"] + coefficient["debufDuration"], character, coefficient["debufPower"]);
        }
        else
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration"], character, coefficient["debufPower"]);
        }
    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach (CharacterBase target in targets)
        {
            if (IsabilityT3_632)
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration"] + coefficient["debufDuration"], character, coefficient["debufPower"]);
            }
            else
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.TargetMarker, coefficient["debufDuration"], character, coefficient["debufPower"]);
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

