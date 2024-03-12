using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAbilityBase
{
    CharacterBase character;

    public virtual void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public virtual void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 적중 시
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }
}
