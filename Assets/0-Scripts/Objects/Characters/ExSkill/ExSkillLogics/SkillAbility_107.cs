using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillAbility_107 : ExSkillLogic
{
    //시스
    //Ex스킬
    //3칸 범위 내의 이동 가능한 한 칸을 선택해 해당 위치로 순간 이동하고, 인접한 8칸 범위 내의
    //가장 체력이 많은 적 1명에게 공격력의 500%  뇌속성 피해를 입힌다.
    //이동 불가능한 칸(적/아군/장애물이 있는 칸)을 선택한 경우, 스킬 시전 불가.

    List<CharacterBase> enemyInRange; //범위 내의 적을 저장할 리스트

    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
        enemyInRange = new List<CharacterBase>();
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {
        if (target.Count > 0)
        {
            // 적 캐릭터의 리스트를 체력을 기준으로 내림차순으로 정렬, 체력이 가장 많은 적을 저장.
            character.targets = new List<CharacterBase> { target.OrderByDescending(enemy => enemy.health.CurHealth).FirstOrDefault() };
            character.characterAnim.SetTarget(target.OrderByDescending(enemy => enemy.health.CurHealth).FirstOrDefault());
        }
    }

    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {
        //스킬 시전 칸으로 이동
        character.MoveTileAndPosition(character.skillScale[0]);

        BattleManager.Instance.EXSkillAttack(character, character.targets);
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
          if (character.skillScale.Count > 0 && character.skillScale[0].curStandingCharater != null)
          {
              character.DeActivateSkill();
          }
          else
          {
              character.ActivateSkill();
          }
    }
}
