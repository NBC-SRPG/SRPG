using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbilityBase
{
    CharacterBase character;

    public virtual void init(CharacterBase character)// �нú� ������ ����
    {
        this.character = character;
    }

    public virtual void OnTurnStart()// �� ���� �� �ߵ�
    {

    }

    public virtual void OnPassAlly(CharacterBase allyCharacter)// �Ʊ� ���� �������� �� �ߵ�
    {

    }

    public virtual void OnAllyPassedMe(CharacterBase allyCharacter)// �Ʊ��� �� ĳ���� ���� �������� �� �ߵ�
    {

    }

    public virtual void OnStartAttack(List<CharacterBase> enemy)// ���� ���� ��
    {

    }

    public virtual void OnAttackSuccess(CharacterBase enemy, int damage)// ���� ���� ��
    {

    }

    public virtual void OnEndAttack(List<CharacterBase> enemy)// ���� ���� ��
    {

    }

    public virtual void OnTakeDamage(CharacterBase enemy)// ���� �޾��� ��
    {

    }

    public virtual void OnStartMoving()// �̵� ��
    {

    }

    public virtual void OnEndMoving()// �̵� ���� ����
    {

    }

    public virtual void OnEndActing()// �ൿ�� ���� ��
    {

    }

    public virtual void OnEndTurn()// ���� ���� ��
    {

    }
}

public class PassiveAbility_ : PassiveAbilityBase // �׽�Ʈ��
{

}