using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterBase : MonoBehaviour
{
    public Character character;
    public string playerId;

    public SkillBase curCharacterSkill;
    public PassiveAbilityBase curCharacterPassive;

    public OverlayTile curStandingTile;
    public int leftWalkRange;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;

    [HideInInspector] public bool didAttack;
    [HideInInspector] public bool didWalk;
    [HideInInspector] public bool canActing;


    public List<OverlayTile> movePath = new List<OverlayTile>();
    public Stack<OverlayTile> pathedTiles = new Stack<OverlayTile>();

    public event Action OnEndWalk;
    public event Action OnEndAttacking;

    //-----------------------------------------------------------------------------------------------------------------------
    // 시작 시 설정
    private void Start()
    {
        character.CharacterInit();

        Managers.MapManager.OnCompleteMove += CheckCurTile;
        Instantiate(character, gameObject.transform);

        //캐릭터 클래스로 부터 스킬을 생성해서 받아옴
        curCharacterSkill = character.InitSkills();
        curCharacterPassive = character.InitPassive();

        SetSkillOwner();

        leftWalkRange = character.Mov;

        isDead = false;
        isWalking = false;
        didAttack = false;
        canActing = true;

        Managers.BattleManager.charactersInBattle.Add(this);
        Managers.BattleManager.charactersAsTeam[playerId].Add(this);
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

    public IEnumerator MoveCharacter()//캐릭터 이동
    {
        OnStartMoving();

        while (movePath.Count > 1)
        {
            while (transform.position != movePath[1].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePath[1].transform.position, 5 * Time.deltaTime);

                yield return null;
            }

            if (transform.position == movePath[1].transform.position)
            {
                pathedTiles.Push(movePath[0]);
                movePath.RemoveAt(0);

                if (movePath[0].curStandingCharater != null)
                {
                    Managers.BattleManager.OnPassCharacter(this, movePath[0].curStandingCharater);

                    yield return new WaitWhile(() => isAttacking);
                }
            }
        }

        if (movePath.Count <= 1 && isWalking)
        {
            while (transform.position != movePath[0].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePath[0].transform.position, 20 * Time.deltaTime);
                yield return null;
            }

            MoveTile(movePath[0]);
            OnEndMoving();
        }
    }

    public void BlockMoving()
    {
        movePath.Clear();

        foreach(OverlayTile tiles in pathedTiles)
        {
            if(tiles.curStandingCharater == null || tiles.curStandingCharater == this)
            {
                movePath.Add(tiles);
                break;
            }
        }
    }


    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    // 전투 관련 함수

    //---------------------------------------------------------------------------
    // 유틸 관련
    public void OnStartTurn()// 턴 시작 시
    {
        isWalking = false;
        isAttacking = false;
        didAttack = false;
        didWalk = false;
        canActing = true;
        leftWalkRange = character.Mov;

        curCharacterPassive?.OnStartTurn();
    }

    public void OnTurnEnd()// 턴 종료 시
    {
        curCharacterPassive?.OnEndTurn();
    }

    public void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnPassAlly(allyCharacter);
    }

    public void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnAllyPassedMe(allyCharacter);
    }
    public void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnEnemyPassesMe(enemyCharacter);
    }

    public void OnStartMoving()// 이동 시
    {
        isWalking = true;

        curCharacterPassive?.OnStartMoving();
    }

    public void OnEndMoving()// 이동 끝난 직후
    {
        Managers.MapManager.CompleteMove();

        curCharacterPassive?.OnEndMoving();
        OnEndWalk?.Invoke();

        isWalking = false;
        didWalk = true;
        if(character.characterData.attackType == Constants.AttackType.Melee)
        {
            didAttack = true;
        }
        pathedTiles.Clear();
        leftWalkRange = 0;

        OnEndActing();
    }

    public void OnEndActing()// 행동이 끝난 뒤
    {
        curCharacterPassive?.OnEndActing();

        CheckingActing();
    }

    public void OnEndTurn()// 턴이 끝날 때
    {
        curCharacterPassive?.OnEndTurn();
    }

    private void CheckingActing()
    {
        if(didAttack && didWalk)
        {
            canActing = false;
        }
    }

    public bool CheckEnenmy(CharacterBase target)
    {
        if(target.playerId != playerId)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------------------
    // 일반 공격 관련

    public void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        curCharacterPassive?.OnStartAttack(enemy);
        isAttacking = true;
    }

    public void OnAttackSuccess(CharacterBase enemy, int damage)// 공격 적중 시
    {
        curCharacterPassive?.OnAttackSuccess(enemy, damage);
    }

    public void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        curCharacterPassive?.OnEndAttack(enemy);

        Invoke(nameof(EndAttacking), 1f);

        OnEndActing();
    }

    private void EndAttacking()
    {
        isAttacking = false;
        if (character.CharacterAttackType == Constants.AttackType.Range)
        {
            didAttack = true;
        }

        OnEndAttacking?.Invoke();
    }

    public void OnTakeDamage(CharacterBase enemy)// 공격 받았을 때
    {
        curCharacterPassive?.OnTakeDamage(enemy);
    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public void OnUseSkill(List<CharacterBase> target)// 스킬 사용 시 
    {
        curCharacterSkill.skillAbility?.OnUseSkill(target);
    }

    public void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 공격 적중 시
    {
        if (curCharacterSkill.skillData.onhit)
        {
            curCharacterPassive?.OnAttackSuccess(target, damage);
        }
        curCharacterSkill.skillAbility?.OnSkillAttackSuccess(target, damage);
    }

    public void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        curCharacterSkill.skillAbility?.OnEndSkill(target);

        didWalk = true;
        didAttack = true;
        OnEndActing();
    }
}
