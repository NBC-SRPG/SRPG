using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_511: PassiveLogic
{
    //"낙뢰"
    //레인의 공격을 받은 적은 10% 확률로 1턴동안 기절하고 잃은 체력 비례 추가 데미지를 입는다.
    //레인의 1 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        int randomValue = UnityEngine.Random.Range(coefficient["randomCons1"], coefficient["randomCons2"]); // 0 ~ 9 반환
        if (randomValue == coefficient["randomCons1"]) // 1/10 확률로 실행
        {
            List<CharacterBase> enemyList = new List<CharacterBase>();
            enemyList.Add(enemy);
            int enemyHealthPercentage = (enemy.health.CurHealth * coefficient["multiplier1"]) / enemy.health.TotalHealth;


            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Stun, coefficient["debufDuration"], character); //1턴 동안 기절 상태이상 적용

            if (enemyHealthPercentage <= coefficient["percentageCondition"]) //체력 50%이하이면
            {
                BattleManager.Instance.ExtraSkillAttack(character, character.Attack * (coefficient["damageCoefficient"]) * coefficient["multiplier2"], enemyList); //추가 데미지 (2배)
            }
            else
            {
                BattleManager.Instance.ExtraSkillAttack(character, character.Attack * (coefficient["damageCoefficient"]), enemyList); //추가 데미지 (1배)
            }
        }

    }
 

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        List<CharacterBase> enemyList = new List<CharacterBase>();
        enemyList.Add(target);

        int randomValue = UnityEngine.Random.Range(coefficient["randomCons1"], coefficient["randomCons2"]); // 0 ~ 9 반환
        if (randomValue == coefficient["randomCons1"]) // 1/10 확률로 실행
        {
            target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Stun, coefficient["debufDuration"], character); //1턴 동안 기절 상태이상 적용
        }
        BattleManager.Instance.ExtraSkillAttack(character, character.Attack * (coefficient["damageCoefficient"]), enemyList); //추가 데미지
    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {

    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }
}

