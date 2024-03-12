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
    public bool isWalking;
    public bool didAttack;

    public List<OverlayTile> movePath = new List<OverlayTile>();

    public event Action OnEndWalk;

    //-----------------------------------------------------------------------------------------------------------------------
    // 시작 시 설정
    private void Start()
    {
        Managers.MapManager.OnCompleteMove += CheckCurTile;
        Instantiate(character, gameObject.transform);

        //캐릭터 클래스로 부터 스킬을 생성해서 받아옴
        curCharacterSkill = character.InitSkills();
        curCharacterPassive = character.InitPassive();

        SetSkillOwner();

        character.CharacterInit();
        leftWalkRange = character.Mov;

        isDead = false;
        isWalking = false;
        didAttack = false;
    }

    //스킬 및 패시브 시전자 설정
    private void SetSkillOwner()
    {
        curCharacterSkill.Init(this);

        curCharacterPassive.init(this);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 이동 관련 함수
    public void CheckCurTile()//현재 타일에 있는 캐릭터 확인
    {
        if (curStandingTile.curStandingCharater == null)
        {
            curStandingTile.curStandingCharater = this;
        }
    }

    public void MoveTile(OverlayTile newTile)//타일 이동
    {
        curStandingTile.curStandingCharater = null;
        curStandingTile = newTile;
        curStandingTile.curStandingCharater = this;
    }

    public void MoveCharacter()
    {
        if (movePath.Count > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePath[1].transform.position, 5 * Time.deltaTime);

            if (transform.position == movePath[1].transform.position)
            {
                movePath.RemoveAt(0);
                Managers.MapManager.CompleteMove();
            }
        }

        if (movePath.Count <= 1 && isWalking)
        {
            MoveTile(movePath[0]);
            movePath.RemoveAt(0);

            OnEndMoving();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    // 전투 관련 함수

    //---------------------------------------------------------------------------
    // 유틸 관련
    public void OnTurnStart()// 턴 시작 시
    {
        isWalking = false;
        didAttack = false;
        curCharacterPassive?.OnTurnStart();
    }

    public void OnTurnEnd()// 턴 종료 시
    {
        curCharacterPassive?.OnEndTurn();
    }

    public virtual void OnPassAlly(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnPassAlly(allyCharacter);
    }

    public virtual void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnAllyPassedMe(allyCharacter);
    }

    public virtual void OnStartMoving()// 이동 시
    {
        curCharacterPassive?.OnStartMoving();
    }

    public virtual void OnEndMoving()// 이동 끝난 직후
    {
        curCharacterPassive?.OnEndMoving();
        OnEndWalk?.Invoke();
        isWalking = false;
    }

    public virtual void OnEndActing()// 행동이 끝난 뒤
    {
        curCharacterPassive?.OnEndActing();
    }

    public virtual void OnEndTurn()// 턴이 끝날 때
    {
        curCharacterPassive?.OnEndTurn();
    }

    //---------------------------------------------------------------------------
    // 일반 공격 관련

    public void OnAttackStart(List<CharacterBase> enemy)// 공격 시작 시
    {
        curCharacterPassive?.OnStartAttack(enemy);
    }

    public virtual void OnAttackSuccess(CharacterBase enemy, int damage)// 공격 적중 시
    {
        curCharacterPassive?.OnAttackSuccess(enemy, damage);
    }

    public virtual void OnEndAttack(List<CharacterBase> enemy)// 공격 종료 시
    {
        curCharacterPassive?.OnEndAttack(enemy);
    }

    public virtual void OnTakeDamage(CharacterBase enemy)// 공격 받았을 때
    {
        curCharacterPassive?.OnTakeDamage(enemy);
    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public virtual void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {
        curCharacterSkill.skillAbility?.OnUseSkill(target);
    }

    public virtual void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 공격 적중 시
    {
        if (curCharacterSkill.skillData.onhit)
        {
            curCharacterPassive?.OnAttackSuccess(target, damage);
        }
        curCharacterSkill.skillAbility?.OnSkillAttackSuccess(target, damage);
    }

    public virtual void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        curCharacterSkill.skillAbility?.OnEndSkill(target);
    }
}
