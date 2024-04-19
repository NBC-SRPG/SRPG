using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_411: PassiveLogic
{
    //"축성의 노래"
    //자신이 치유한 유닛 1명 당 코스트를 5 획득한다.
    //포르테의 1 특성.
    //로직 추가 필요. (머지 후)


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        //여기에 코스트 회복 로직 넣기
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public override void OnUpdate()// 실시간 판정
    {

    }
}

