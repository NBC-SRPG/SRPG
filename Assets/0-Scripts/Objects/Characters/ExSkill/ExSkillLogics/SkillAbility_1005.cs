using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SkillAbility_1005 : ExSkillLogic
{
    public override void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public override void UseSkill(List<CharacterBase> targets)// 스킬 실제 사용
    {
        BattleManager.Instance.EXSkillAttack(character, targets);
    }
}
