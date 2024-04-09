using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExSkillLogic
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

    public virtual void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }

    public virtual void OnUpdate()// 실시간 판정
    {

    }
}

public class SkillAbility_101 : ExSkillLogic // 테스트용(테스트 끝나면 비어있는 스킬로 활용)
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
            target2.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Herald, 3, this.character);
        }
    }

    //public override void OnUpdate()// 스킬 타겟 위치에 적이 있다면, 스킬 사용 불가능하도록 테스트
    //{
    //    base.OnUpdate();

    //    if (character.skillScale.Count > 0 && character.skillScale[0].curStandingCharater != null)
    //    {
    //        character.DeActivateSkill();
    //    }
    //    else
    //    {
    //        character.ActivateSkill();
    //    }
    //}
}

public class SkillAbility_102 : ExSkillLogic // 테스트용(테스트 끝나면 비어있는 스킬로 활용)
{
    public override void init(CharacterBase character)
    {
        base.init(character);
    }

    public override void UseSkill(List<CharacterBase> target)
    {
        base.UseSkill(target);

        foreach (CharacterBase target2 in target)
        {
            target2.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Herald, 3, this.character);
        }
    }
}