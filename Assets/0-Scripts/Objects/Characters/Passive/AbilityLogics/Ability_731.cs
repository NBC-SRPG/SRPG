using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_731: PassiveLogic
{
    //"한번더!!!"
    //Ex 스킬로 대상을 처치하지 못했다면, 턴 종료시에 50% 확률로 대상이 잃은 체력의 15% + 시스 공격력의 250% 피해를 입힌다.
    //시스의 3-1 특성

    CharacterBase skillTarget;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        if(target == null)
        {
            skillTarget = null;
        }

        if (!target.isDead)
        {
            skillTarget = target;
        }
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public override void OnTurnEnd()// 턴이 끝날 때
    {
        int ran = Random.Range(0, 2);

        if(skillTarget != null && !skillTarget.isDead && ran == 0)
        {
            int damageAmount = (int)((skillTarget.health.TotalHealth - skillTarget.health.CurHealth) * ((float)(coefficient["healthRate"]) / coefficient["denominator"])); //잃은 체력을 구한다
            BattleManager.Instance.ExtraSkillAttack(character, (int)((float)character.Attack * ((float)(coefficient["damageCoefficient"]) / coefficient["denominator"])) + damageAmount, new List<CharacterBase> { skillTarget}); //추가 데미지
        }

        skillTarget = null;
    }

}

