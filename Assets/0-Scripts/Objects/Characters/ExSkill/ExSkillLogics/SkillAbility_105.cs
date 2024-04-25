using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_105 : ExSkillLogic
{
    //레인
    //Ex스킬
    //지정 범위 마름모꼴 13칸의 적을 공격하고 [부식] 디버프를 3회 중첩한다. (부식은 패시브에서 겁니다.)
    //현재 스킬 계수는 200으로 설정되어있음.

    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public override void UseSkill(List<CharacterBase> targets)// 스킬 실제 사용
    {
        BattleManager.Instance.EXSkillAttack(character, targets);// 첫 타는 스킬 패시브 발동
        BattleManager.Instance.ExtraSkillAttack(character, character.curCharacterSkill.SkillFigure, targets, BattleKeyWords.AttackDamageType.None);// 나머지는 스킬 패시브 발동 안되는 데미지
        BattleManager.Instance.ExtraSkillAttack(character, character.curCharacterSkill.SkillFigure, targets, BattleKeyWords.AttackDamageType.None);
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
