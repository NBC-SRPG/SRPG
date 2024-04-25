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

    BonusStat stat_109;


    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
        
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public override void UseSkill(List<CharacterBase> targets)// 스킬 실제 사용
    {
        character.MoveTileAndPosition(character.rangeFinder.GetBackOfCharacter(character, character.skillScale[0].curStandingCharater));

        //스킬 사용시 타겟의 체력에 따라서 일시적으로 데미지 보너스를 얻는다.
        //스킬 피해가 250 % ~ 500%의 광속성 피해이고 기본 계수가 250%이므로, 데미지 증가 보너스 배율값은 적의 체력이 0%일 때 2.1f까지 늘어난다. 
        //예시1) 대상의 체력이 30% 남았을 때, 80% 데미지 증가 버프.
        //예시2) 대상의 체력이 10% 남았을 때, 100% 데미지 증가 버프.
        float damageMultiplier = ((1 - ((float)(targets[0].health.CurHealth) / targets[0].health.TotalHealth)) + 0.1f);

        BonusStat stat_109 = new BonusStat();
        stat_109.EnhancedDmg = damageMultiplier;
        character.tempBonusStat.AddBonusStat(stat_109);

        BattleManager.Instance.EXSkillAttack(character, targets);
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {

    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_109);
    }

    public override void OnUpdate()// 실시간 판정
    {
        if (character.skillScale.Count > 0 && character.skillScale[0].curStandingCharater.CheckEnemy(character) &&
            character.rangeFinder.GetBackOfCharacter(character, character.skillScale[0].curStandingCharater) != null &&
            character.rangeFinder.GetBackOfCharacter(character, character.skillScale[0].curStandingCharater).canClick &&
            character.rangeFinder.GetBackOfCharacter(character, character.skillScale[0].curStandingCharater).CheckCanMove())
        {
            character.ActivateSkill();
        }
        else
        {
            character.DeActivateSkill();
        }
    }
}
