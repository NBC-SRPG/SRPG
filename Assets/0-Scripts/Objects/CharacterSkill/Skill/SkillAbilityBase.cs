using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAbilityBase
{
    protected CharacterBase character;

    public virtual void init(CharacterBase character)// 스킬 소유자 설정
    {
        this.character = character;
    }

    public virtual void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {

    }

    public virtual void UseSkill(List<CharacterBase> target)// 스킬 실제 사용
    {

    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 적중 시
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }
}

public class SkillAbility_ : SkillAbilityBase // 테스트용
{
    public override void init(CharacterBase character)
    {
        base.init(character);
    }

    public override void UseSkill(List<CharacterBase> target)
    {
        base.UseSkill(target);

        foreach(CharacterBase target2 in target)
        {
            target2.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Test_UniqBuf, 3, this.character);
        }
    }
}