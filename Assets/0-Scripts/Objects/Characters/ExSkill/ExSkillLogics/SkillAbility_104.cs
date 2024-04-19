using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_104 : ExSkillLogic
{
    //포르테
    //Ex스킬
    //지정 범위 사각형 9칸의 적과 아군을 모두 치유한다.

    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    //문제 사항: 포르테의 Ex스킬 힐은 최대 체력 계수인데, EXSkillHeal 메서드의 figure는 공격력 기반 계산식만 존재함.
    //그래서 EXSkillBase에 체력 계수 스킬 Figure 필드를 추가하고 EXSkillHeal의 계수를 체력 계수 Figure로 바꿨습니다.
    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {
        BattleManager.Instance.EXSkillHeal(character, target);
    }
}
