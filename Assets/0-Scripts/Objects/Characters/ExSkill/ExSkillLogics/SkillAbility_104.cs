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

    public override void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {
        BattleManager.Instance.EXSkillHeal(character, target);
    }
}
