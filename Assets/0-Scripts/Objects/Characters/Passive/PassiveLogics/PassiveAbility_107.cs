using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_107 : PassiveLogic
{
    //ID 007. 시스 Sis
    //패시브 스킬
    //시스의 일반 공격 및 스킬 공격에 피격당한 적의 체력이 이번 공격으로 50% 이상에서 50% 미만으로 내려간 경우, 대상에게 1턴 동안 [기절]을 부여한다.

    int enemyPrevHp;
    BonusStat stat_107;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

    }

    public override void OnStageStart()
    {
        if (character.character.abilityT2.id == coefficient["abilityId_T2"])
        {
            stat_107 = new BonusStat();
            character.tempBonusStat.AddBonusStat(stat_107);
        }
    }

    public override void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        enemyPrevHp = enemy.health.CurHealth;
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {

    }

    public override void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        //대상의 피격 이전 체력에 50% 이상이고 피격 이후 체력이 50% 미만인 경우
        if (enemyPrevHp >= (enemy.health.TotalHealth / (float)coefficient["divider"]) && enemy.health.CurHealth < (enemy.health.TotalHealth / (float)coefficient["divider"]) && !enemy.isDead)
        {
            //1턴 동안 기절 적용
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Stun, coefficient["debufDuration"], character);

            //적에게 기절을 거는 데 성공 && 특성 722 "연계공격" 적용 중일 경우
            if(enemy.curCharacterBufList.FindBuf(BattleKeyWords.BufKeyword.Stun, character) != null && character.character.abilityT2.id == coefficient["abilityId_T2"])
            {
                //stat_107에 0.1을 더한다.
                stat_107.ExtraAtk += (float)(coefficient["atkIncreaseRatio"]) / coefficient["denominator"]; 
            }
        }
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {
        foreach (CharacterBase target in targets)
        {
            enemyPrevHp = target.health.CurHealth;
        }
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> targets)// 스킬 사용 종료 시
    {
        foreach (CharacterBase target in targets)
        {
            //대상의 피격 이전 체력에 50% 이상이고 피격 이후 체력이 50% 미만인 경우
            if (enemyPrevHp >= (target.health.TotalHealth / (float)coefficient["divider"]) && target.health.CurHealth < (target.health.TotalHealth / (float)coefficient["divider"]) && !target.isDead)
            {
                //1턴 동안 기절 적용
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Stun, coefficient["debufDuration"], character);

                //적에게 기절을 거는 데 성공 && 특성 722 "연계공격" 적용 중일 경우
                if (target.curCharacterBufList.FindBuf(BattleKeyWords.BufKeyword.Stun, character) != null && character.character.abilityT2.id == coefficient["abilityId_T2"])
                {
                    //stat_107에 0.1을 더한다.
                    stat_107.ExtraAtk += (float)(coefficient["atkIncreaseRatio"]) / coefficient["denominator"];
                }
            }
        }
    }
}

