using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_106 : ExSkillLogic
{
    //피유
    //Ex스킬
    //6칸 범위 내의 단일 대상을 선택해 피유 공격력의 300%  풀 속성 피해를 입힌다. 

    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {
        BattleManager.Instance.EXSkillAttack(character, target);
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

    }
}
