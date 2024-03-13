using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterBase : MonoBehaviour
{
    public Character character;

    public SkillBase curCharacterSkill;
    public PassiveAbilityBase curCharacterPassive;

    public OverlayTile curStandingTile;
    public int leftWalkRange;
    [HideInInspector] public bool isDead;

    public List<OverlayTile> movePath = new List<OverlayTile>();

    //-----------------------------------------------------------------------------------------------------------------------
    // ���� �� ����
    private void Start()
    {
        Managers.MapManager.OnCompleteMove += CheckCurTile;
        Instantiate(character, gameObject.transform);

        //ĳ���� Ŭ������ ���� ��ų�� �����ؼ� �޾ƿ�
        curCharacterSkill = character.InitSkills();
        curCharacterPassive = character.InitPassive();

        SetSkillOwner();
    }

    //��ų �� �нú� ������ ����
    private void SetSkillOwner()
    {
        curCharacterSkill.Init(this);

        curCharacterPassive.init(this);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // �̵� ���� �Լ�
    public void CheckCurTile()
    {
        if (curStandingTile.curStandingCharater == null)
        {
            curStandingTile.curStandingCharater = this;
        }
    }

    public void MoveTile(OverlayTile newTile)
    {
        curStandingTile.curStandingCharater = null;
        curStandingTile = newTile;
        curStandingTile.curStandingCharater = this;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    // ���� ���� �Լ�

    //---------------------------------------------------------------------------
    // ��ƿ ����
    public void OnTurnStart()// �� ���� ��
    {
        curCharacterPassive?.OnTurnStart();
    }

    public void OnTurnEnd()// �� ���� ��
    {
        curCharacterPassive?.OnEndTurn();
    }

    public virtual void OnPassAlly(CharacterBase allyCharacter)// �Ʊ� ���� �������� �� �ߵ�
    {
        curCharacterPassive?.OnPassAlly(allyCharacter);
    }

    public virtual void OnAllyPassedMe(CharacterBase allyCharacter)// �Ʊ��� �� ĳ���� ���� �������� �� �ߵ�
    {
        curCharacterPassive?.OnAllyPassedMe(allyCharacter);
    }

    public virtual void OnStartMoving()// �̵� ��
    {
        curCharacterPassive?.OnStartMoving();
    }

    public virtual void OnEndMoving()// �̵� ���� ����
    {
        curCharacterPassive?.OnEndMoving();
    }

    public virtual void OnEndActing()// �ൿ�� ���� ��
    {
        curCharacterPassive?.OnEndActing();
    }

    public virtual void OnEndTurn()// ���� ���� ��
    {
        curCharacterPassive?.OnEndTurn();
    }

    //---------------------------------------------------------------------------
    // �Ϲ� ���� ����

    public void OnAttackStart(List<CharacterBase> enemy)// ���� ���� ��
    {
        curCharacterPassive?.OnStartAttack(enemy);
    }

    public virtual void OnAttackSuccess(CharacterBase enemy, int damage)// ���� ���� ��
    {
        curCharacterPassive?.OnAttackSuccess(enemy, damage);
    }

    public virtual void OnEndAttack(List<CharacterBase> enemy)// ���� ���� ��
    {
        curCharacterPassive?.OnEndAttack(enemy);
    }

    public virtual void OnTakeDamage(CharacterBase enemy)// ���� �޾��� ��
    {
        curCharacterPassive?.OnTakeDamage(enemy);
    }

    //---------------------------------------------------------------------------
    // ��ų ����

    public virtual void OnUseSkill(List<CharacterBase> target)// ��ų ��� �� 
    {
        curCharacterSkill.skillAbility?.OnUseSkill(target);
    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// ��ų ���� ���� ��
    {
        if (curCharacterSkill.skillData.onhit)
        {
            curCharacterPassive?.OnAttackSuccess(target, damage);
        }
        curCharacterSkill.skillAbility?.OnSkillAttackSuccess(target, damage);
    }

    public virtual void OnEndSkill(List<CharacterBase> target)// ��ų ��� ���� ��
    {
        curCharacterSkill.skillAbility?.OnEndSkill(target);
    }
}
