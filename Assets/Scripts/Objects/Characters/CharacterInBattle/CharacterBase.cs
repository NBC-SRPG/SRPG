using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterBase : MonoBehaviour
{
    public Character character;
    public CharAnimBase characterAnim;
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
    [HideInInspector] public bool didUseSkill;

    [HideInInspector] public bool canSkill;
    [HideInInspector] public bool canActing;

    [HideInInspector] public List<OverlayTile> skillScale = new List<OverlayTile>();
    [HideInInspector] public List<OverlayTile> movePath = new List<OverlayTile>();
    private Stack<OverlayTile> pathedTiles = new Stack<OverlayTile>();

    public List<CharacterBase> targets = new List<CharacterBase>();
    public CharacterBase target;

    public event Action OnEndWalk;
    public event Action OnEndAttacking;
    public event Action OnEndUseSkill;

    private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    //-----------------------------------------------------------------------------------------------------------------------
    // 시작 시 설정

    public void InitCharacter(Character charac, string id)
    {
        character = charac;
        playerId = id;

        Managers.MapManager.OnCompleteMove += CheckCurTile;
        character = Instantiate(character, gameObject.transform);

        character.CharacterInit();

        characterAnim = GetComponentInChildren<CharAnimBase>();
        characterAnim.Init(this);

        //캐릭터 클래스로 부터 스킬을 생성해서 받아옴
        curCharacterSkill = character.InitSkills();
        curCharacterPassive = character.InitPassive();

        SetSkillOwner();

        leftWalkRange = character.Mov;

        isDead = false;
        isWalking = false;

        didAttack = false;
        didWalk = false;
        didUseSkill = false;

        canSkill = false;
        canActing = false;

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
        CameraController.instance.SetCameraOnCharacter(this);

        while (movePath.Count > 1)
        {
            while (transform.position != movePath[1].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePath[1].transform.position, 5 * Time.deltaTime);
                characterAnim.FlipCharacter(movePath[1].transform.position, false);

                yield return null;
            }

            if (transform.position == movePath[1].transform.position)
            {
                pathedTiles.Push(movePath[0]);
                movePath.RemoveAt(0);

                if (movePath[0].curStandingCharater != null)
                {
                    target = movePath[0].curStandingCharater;
                    MoveTile(movePath[0]);
                    StartCoroutine(Managers.BattleManager.OnPassCharacter(this, target));

                    yield return animationWait;
                }
                else
                {
                    MoveTile(movePath[0]);
                }
            }
        }

        if (movePath.Count <= 1 && isWalking)
        {
            while (transform.position != movePath[0].transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, movePath[0].transform.position, 20 * Time.deltaTime);
                characterAnim.FlipCharacter(movePath[0].transform.position, true);

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
        didUseSkill = false;

        canSkill = true;
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

    public void OnPassEnemy(CharacterBase enemyCharacter)// 적군 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnPassEnemy(enemyCharacter);
    }

    public void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnEnemyPassesMe(enemyCharacter);
    }

    public void OnStartMoving()// 이동 시
    {
        isWalking = true;
        characterAnim.PlayMoveAnimation();

        curCharacterPassive?.OnStartMoving();
    }

    public void OnEndMoving()// 이동 끝난 직후
    {
        Managers.MapManager.CompleteMove();

        curCharacterPassive?.OnEndMoving();

        isWalking = false;
        didWalk = true;
        if(character.characterData.attackType == Constants.AttackType.Melee)
        {
            didAttack = true;
        }
        pathedTiles.Clear();
        leftWalkRange = 0;

        characterAnim.EndAnimation();
        OnEndActing();

        OnEndWalk?.Invoke();
    }

    public void OnEndActing()// 행동이 끝난 뒤
    {
        curCharacterPassive?.OnEndActing();

        target = null;
        targets.Clear();

        CheckingActing();
    }

    public void OnEndTurn()// 턴이 끝날 때
    {
        curCharacterPassive?.OnEndTurn();
    }

    private void CheckingActing()// 행동 가능 횟수 확인
    {
        if(didAttack || didWalk)
        {
            canSkill = false;
        }

        if((didAttack && didWalk) || didUseSkill)
        {
            canSkill = false;
            canActing = false;
        }
    }

    public bool CheckEnenmy(CharacterBase target)// 적인지 확인
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

    public void AttackTarget()// 캐릭터 공격
    {
         Managers.BattleManager.Attack(this, target);
    }

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

        StartCoroutine(nameof(EndAttacking));
    }

    private IEnumerator EndAttacking()// 공격 끝내기(컷씬 등의 재생 이후)
    {
        yield return animationWait;

        isAttacking = false;
        if (character.CharacterAttackType == Constants.AttackType.Range)
        {
            didAttack = true;

            OnEndActing();
        }

        OnEndAttacking?.Invoke();
    }

    public void OnTakeDamage(CharacterBase enemy)// 공격 받았을 때
    {
        curCharacterPassive?.OnTakeDamage(enemy);
    }

    private void GetAttackTarget()// 공격 타겟 가져오기
    {
        List<OverlayTile> temp = new List<OverlayTile>();

        temp = skillScale.FindAll(x => x.curStandingCharater != null && x.curStandingCharater.CheckEnenmy(this));

        foreach (OverlayTile scale in temp)
        {
            targets.Add(scale.curStandingCharater);
        }
    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public void UseSkill()// 스킬 사용
    {
        GetSkillTarget();

        StartCoroutine(Managers.BattleManager.UseSkill(this, targets));
    }

    private void GetSkillTarget()// 스킬 타겟 가져오기
    {
        List<OverlayTile> temp = new List<OverlayTile>();
        curCharacterSkill.targetTiles = skillScale;

        switch (curCharacterSkill.skillData.targetType)
        {
            case Constants.SkillTargetType.Me:
                targets.Add(this);
                break;
            case Constants.SkillTargetType.Enemy:
                temp = skillScale.FindAll(x => x.curStandingCharater != null && x.curStandingCharater.CheckEnenmy(this));
                break;
            case Constants.SkillTargetType.Ally:
                temp = skillScale.FindAll(x => x.curStandingCharater != null && !x.curStandingCharater.CheckEnenmy(this));
                break;
            case Constants.SkillTargetType.All:
                temp = skillScale.FindAll(x => x.curStandingCharater != null);
                break;
            case Constants.SkillTargetType.AllExceptME:
                temp = skillScale.FindAll(x => x.curStandingCharater != null && x.curStandingCharater != this);
                break;
        }

        foreach (OverlayTile scale in temp)
        {
            targets.Add(scale.curStandingCharater);
        }
    }

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

        Invoke(nameof(EndUseSkill), 1f);
    }

    private void EndUseSkill()// 스킬 끝내기(컷씬 등의 재생 이후)
    {
        didUseSkill = true;

        OnEndActing();

        OnEndUseSkill?.Invoke();
    }
}
