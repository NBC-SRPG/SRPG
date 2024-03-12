using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAbilityBase
{
    CharacterBase character;

    public virtual void init(CharacterBase character)// ��ų ������ ����
    {
        this.character = character;
    }

    public virtual void OnUseSkill(List<CharacterBase> target)// ��ų ��� �� 
    {

    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// ��ų ���� ��
    {

    }

    public virtual void OnEndSkill(List<CharacterBase> target)// ��ų ��� ���� ��
    {

    }
}

public class SkillAbility_ : SkillAbilityBase // �׽�Ʈ��
{
    public override void init(CharacterBase character)
    {
        base.init(character);
        Debug.Log("testSkill Init");
    }
}
