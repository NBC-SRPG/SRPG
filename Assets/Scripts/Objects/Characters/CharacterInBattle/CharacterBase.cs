using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterBase : MonoBehaviour
{
    public Character character;
    public GameObject characterObject;
    public CharAnimBase characterAnim;
    public HealthSystem health;
    public string playerId;

    public SkillBase curCharacterSkill;
    public PassiveAbilityBase curCharacterPassive;

    public CharacterBufList curCharacterBufList;

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

    [HideInInspector] public bool hasAnimationBeforDIe = false;

    [HideInInspector] public List<OverlayTile> skillScale = new List<OverlayTile>();
    public List<OverlayTile> movePath = new List<OverlayTile>();
    private Stack<OverlayTile> pathedTiles = new Stack<OverlayTile>();

    public List<CharacterBase> targets = new List<CharacterBase>();
    public CharacterBase target;

    public event Action OnEndWalk;
    public event Action OnEndAttacking;
    public event Action OnEndUseSkill;

    //private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    //-----------------------------------------------------------------------------------------------------------------------
    // 시작 시 설정

    public void InitCharacter(Character charac, string id)
    {
        character = charac;
        playerId = id;

        Managers.MapManager.OnCompleteMove += CheckCurTile;
        character = Instantiate(character, gameObject.transform);
        characterObject = character.gameObject;

        character.CharacterInit();

        characterAnim = GetComponentInChildren<CharAnimBase>();
        characterAnim.Init(this);

        health = GetComponent<HealthSystem>();
        health.SetHealth(character.Health);
        health.Die += CharacterDie;
        health.DieAnimation += DieAnimation;

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

        AnimationController.instance.onAnimationEnd += CheckActivated;
    }

    //스킬 및 패시브 시전자 설정
    private void SetSkillOwner()
    {
        curCharacterSkill.Init(this);

        curCharacterPassive.init(this);

        curCharacterBufList = new CharacterBufList(this);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 이동 관련 함수
    public void CheckCurTile()//현재 타일에 있는 캐릭터 확인
    {
        if (curStandingTile != null && curStandingTile.curStandingCharater == null)
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

    public void MoveCharacter()//캐릭터 이동
    {
        curStandingTile.curStandingCharater = null;
        OnStartMoving();
        int i = 0;

        while (i < movePath.Count)// 이동 가능할 때
        {
            AnimationController.instance.EnqueueMoveAnimation(this, curStandingTile, movePath[i]);

            curStandingTile = movePath[i];
            pathedTiles.Push(movePath[i]);

            if (movePath[i].curStandingCharater != null)
            {
                target = movePath[i].curStandingCharater;
                Managers.BattleManager.OnPassCharacter(this, target);
            }

            if (isDead)
            {
                BlockMoving();
                break;
            }

            i++;
        }

        MoveTile(curStandingTile);

        OnEndMoving();

        if (isDead)
        {
            OnDie();
        }

        AnimationController.instance.StartAnimationQueue();
    }

    public void BlockMoving()//이동 막힘
    {
        movePath.Clear();

        OverlayTile prevTile = pathedTiles.First();

        foreach(OverlayTile tile in pathedTiles)//지나간 타일에서 비어있는 타일 선택
        {
            if(tile.curStandingCharater == null || tile.curStandingCharater == this)
            {
                curStandingTile = tile;

                AnimationController.instance.EnqueueBackAnimation(this, prevTile, tile);

                break;
            }
        }
    }


    //-----------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
    // 전투 관련 함수

    //---------------------------------------------------------------------------
    // 유틸 관련
    public void OnRoundStart()
    {
        curCharacterPassive?.OnRoundStart();
    }

    public void OnStartPlayerTurn()// 턴 시작 시
    {
        isWalking = false;
        isAttacking = false;

        didAttack = false;
        didWalk = false;
        didUseSkill = false;

        canSkill = true;
        canActing = true;

        leftWalkRange = character.Mov;

        curCharacterPassive?.OnTurnStart();
        curCharacterBufList?.OnTurnStart();
    }

    public void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnPassAlly(allyCharacter);
        curCharacterBufList?.OnPassAlly(allyCharacter);
    }

    public void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnAllyPassedMe(allyCharacter);
        curCharacterBufList?.OnAllyPassedMe(allyCharacter);
    }

    public void OnPassEnemy(CharacterBase enemyCharacter)// 적군 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnPassEnemy(enemyCharacter);
        curCharacterBufList?.OnPassEnemy(enemyCharacter);
    }

    public void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {
        curCharacterPassive?.OnEnemyPassesMe(enemyCharacter);
        curCharacterBufList?.OnEnemyPassesMe(enemyCharacter);
    }

    public void OnStartMoving()// 이동 시
    {
        isWalking = true;

        GetAttackTarget();

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

        movePath.Clear();
        pathedTiles.Clear();
        leftWalkRange = 0;

        OnEndActing();

        OnEndWalk?.Invoke();
    }

    public void OnEndActing()// 행동이 끝난 뒤
    {
        curCharacterPassive?.OnEndActing();
        curCharacterBufList?.OnEndActing();

        if (isDead)
        {
            OnDie();
        }

        target = null;
        targets.Clear();

        CheckingActing();
    }

    public void OnEndPlayerTurn()// 플레이어 턴이 끝날 때
    {
        characterAnim.Activate();

        curCharacterPassive?.OnTurnEnd();
        curCharacterBufList?.OnTurnEnd();
    }

    public void OnRoundEnd()// 턴이 끝날 때
    {
        curCharacterPassive?.OnRoundEnd();
        curCharacterBufList?.OnRoundEnd();
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

    private void CheckActivated()
    {
        if (!canActing && Managers.BattleManager.nowPlayer.playerId == playerId)
        {
            characterAnim.DeActivate();
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

    public void SetAttackTarget(CharacterBase enemy)// 공격 시작
    {
        target = enemy;
        Managers.BattleManager.Attack(this, target);
    }

    public void TakeAttacked(CharacterBase enemy)// 공격 대상이 되었을 때
    {
        curCharacterPassive?.OnTakeAttacked(enemy);
    }

    public void AttackTarget(CharacterBase enemy)// 캐릭터 공격
    {
        target = enemy;
        Managers.BattleManager.DoAttack(this, target);
    }

    public void CounterAttack(CharacterBase enemy)
    {
        target = enemy;
        Managers.BattleManager.CounterAttack(this, target);
    }

    public void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        curCharacterPassive?.OnStartAttack(enemy);
        curCharacterBufList?.OnStartAttack(enemy);

        isAttacking = true;
    }

    public void OnAttackSuccess(CharacterBase enemy, int damage)// 공격 적중 시
    {
        curCharacterPassive?.OnAttackSuccess(enemy, damage);
        curCharacterBufList?.OnAttackSuccess(enemy, damage);
    }

    public void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        curCharacterPassive?.OnEndAttack(enemy);
        curCharacterBufList?.OnEndAttack(enemy);
        
        EndAttacking();
    }

    private void EndAttacking()// 공격 끝내기
    {
        isAttacking = false;
        if (character.CharacterAttackType == Constants.AttackType.Range)
        {
            didAttack = true;

            OnEndActing();
        }
    }

    public void OnTakeDamage(CharacterBase enemy)// 공격 받았을 때
    {
        curCharacterPassive?.OnTakeDamage(enemy);
        curCharacterBufList?.OnTakeDamage(enemy);
    }

    private void GetAttackTarget()// 공격 타겟 가져오기
    {
        List<OverlayTile> temp = new List<OverlayTile>();

        temp = movePath.FindAll(x => x.curStandingCharater != null && x.curStandingCharater.CheckEnenmy(this));

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

        Managers.BattleManager.UseSkill(this, targets);
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
        curCharacterPassive.OnUseSkill(target);
        curCharacterSkill.skillAbility?.OnUseSkill(target);
    }

    public void OnSkillAttackSuccess(CharacterBase target, int damage)// 스킬 공격 적중 시
    {
        if (curCharacterSkill.skillData.onhit)
        {
            curCharacterPassive?.OnAttackSuccess(target, damage);
        }
        curCharacterPassive.OnSkillAttackSuccess(target, damage);
        curCharacterSkill.skillAbility?.OnSkillAttackSuccess(target, damage);
    }

    public void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        curCharacterPassive.OnEndSkill(target);
        curCharacterSkill.skillAbility?.OnEndSkill(target);
        
        EndUseSkill();
    }

    private void EndUseSkill()// 스킬 끝내기(컷씬 등의 재생 이후)
    {
        didUseSkill = true;

        OnEndActing();
    }

    //---------------------------------------------------------------------------
    // 캐릭터 사망 시

    private void CharacterDie()// 캐릭터 사망
    {
        isDead = true;

        //AnimationController.instance.EnqueueDieAnimation(this);
        //AnimationController.instance.StartAnimationQueue();
    }

    private void DieAnimation()
    {
        AnimationController.instance.StartDieAnimation(this);
    }

    public void OnDieInBattle(CharacterBase killer)// 전투 중 사망 시
    {
        curCharacterPassive?.OnDieInBattle(killer);
        curCharacterPassive?.OnDie();

        OnDie();
    }

    public void OnDie()// 사망 시
    {
        curCharacterPassive?.OnDie();
        curCharacterBufList?.OnDie();

        Debug.Log("die");

        Managers.BattleManager.CheckRemainCharacter();
    }

    private void OnDisable()
    {
        curStandingTile.curStandingCharater = null;
        curStandingTile = null;
    }
}
