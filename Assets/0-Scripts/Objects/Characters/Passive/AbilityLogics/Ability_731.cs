using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_731: PassiveLogic
{
    //"한번더!!!"
    //Ex 스킬로 대상을 처치하지 못했다면, 턴 종료시에 50% 확률로 대상이 잃은 체력의 15% + 시스 공격력의 250% 피해를 입힌다.
    //시스의 3-1 특성

    List<CharacterBase> skillTarget;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        if (!target.isDead)
        {
            skillTarget.Clear();
            skillTarget.Add(target);
        }
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public override void OnTurnEnd()// 턴이 끝날 때
    {
        if(skillTarget.Count > 0 && !skillTarget[0].isDead)
        {
            int damageAmount = (int)((skillTarget[0].health.TotalHealth - skillTarget[0].health.CurHealth) * ((float)(coefficient["healthRate"] / coefficient["denominator"]))); //잃은 체력을 구한다
            BattleManager.Instance.ExtraSkillAttack(character, character.Attack * (coefficient["damageCoefficient"]) + damageAmount, skillTarget); //추가 데미지
        }

        skillTarget.Clear();
    }

}

