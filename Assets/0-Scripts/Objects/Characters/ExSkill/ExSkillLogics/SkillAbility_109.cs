using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_109 : ExSkillLogic
{
    //E-03
    //Ex스킬
    //지정한 적의 뒤로 순간 이동할 수 있을 때만 시전할 수 있다.
    ///적의 뒤로 순간 이동해서,
    //대상의 잃은 체력에 비례해 E-03 공격력의 250 % ~ 500% 만큼 광속성 피해를 입힌다.




    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {

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

    public override void OnUpdate()// 실시간 판정
    {
        if (character.skillScale[0].curStandingCharater.CheckEnemy(character))
        {
            character.DeActivateSkill();
        }
        else
        {
            character.ActivateSkill();
        }
    }
}
