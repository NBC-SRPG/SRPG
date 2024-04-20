using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Character character;
    public GameObject characterObject;
    public CharAnimBase characterAnim;
    public HealthSystem health;
    public GamePlayer player;
    public string playerId;

    public ExSkillBase curCharacterSkill;
    public List<PassiveLogic> curCharacterPassive;

    public CharacterBufList curCharacterBufList;
    public TempBonusStat tempBonusStat;

    public CharacterHistory historyCurrentRound;// 이번 라운드에 했던 행동들
    public CharacterHistory historyPrevRound;// 이전 라운드에 했던 행동들

    public OverlayTile curStandingTile;
    [HideInInspector] public int leftWalkRange;

    [HideInInspector] public int skillCost;

    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;

    [HideInInspector] public bool didAttack;
    [HideInInspector] public bool didWalk;
    [HideInInspector] public bool didUseSkill;

    [HideInInspector] public bool canSkill;
    [HideInInspector] public bool canActing;
    [HideInInspector] public bool canMoveSkil;

    [HideInInspector] public bool hasAnimationBeforDIe = false;
    private bool onDiePassive = false;

    [HideInInspector] public List<OverlayTile> skillScale = new List<OverlayTile>();
    public List<OverlayTile> movePath = new List<OverlayTile>();
    private Stack<OverlayTile> pathedTiles = new Stack<OverlayTile>();

    public List<CharacterBase> targets = new List<CharacterBase>();
    public CharacterBase target;

    public event Action OnEndWalk;
    public event Action OnEndAttacking;
    public event Action OnEndUseSkill;

    public PathFinder pathFinder;
    public RangeFinder rangeFinder;

    //private WaitWhile animationWait = new WaitWhile(() => AnimationController.instance.isAnimationPlaying);

    //-----------------------------------------------------------------------------------------------------------------------
    // 시작 시 설정

    public void SpawnCharacter(OverlayTile spawnPosition, Transform parent, Vector2 direction)
    {
        gameObject.SetActive(true);
        transform.SetParent(parent);

        curStandingTile = spawnPosition;
        curStandingTile.curStandingCharater = this;

        transform.position = curStandingTile.transform.position;

        BattleManager.Instance.charactersInBattle.Add(this);
        BattleManager.Instance.charactersAsTeam[playerId].Add(this);

        characterAnim.FlipCharacterDirection(direction);
        characterAnim.Activate();
    }

    public virtual void InitCharacter(Character charac, GamePlayer gamePlayer)
    {
        character = charac;
        player = gamePlayer;
        playerId = gamePlayer.playerId;

        MapManager.instance.OnCompleteMove += CheckCurTile;
        characterObject = Managers.Resource.Instantiate("character", transform);
        characterObject.GetComponent<Animator>().runtimeAnimatorController = Managers.Resource.Load<AnimatorOverrideController>("Animation/" + character.SO.id);
        characterObject.AddComponent(Type.GetType("CharAnim_" + character.SO.animatorName));

        // 캐릭터 클래스로 부터 Ex스킬을 생성해서 받아옴
        curCharacterSkill = new ExSkillBase(character.exSkill);
        curCharacterPassive = new List<PassiveLogic>();

        PassiveLoad();
        SetSkillOwner();

        leftWalkRange = Mov;
        skillCost = curCharacterSkill.skillData.cost;

        isDead = false;
        isWalking = false;

        didAttack = false;
        didWalk = false;
        didUseSkill = false;

        canSkill = false;
        canActing = false;

        AnimationController.instance.onAnimationEnd += CheckActivated;

        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();

        health = GetComponentInChildren<HealthSystem>();
        characterAnim = GetComponentInChildren<CharAnimBase>();

        health.InitHealth(character.hp, tempBonusStat, curCharacterBufList, characterAnim);
        characterAnim.Init(health);

        health.Die += CharacterDie;
        health.DieAnimation += DieAnimation;

        historyCurrentRound = new CharacterHistory();

        gameObject.SetActive(false);
    }

    // 캐릭터가 가질 수 있는 모든 패시브 효과 추가
    private void PassiveLoad()
    {
        PassiveSO[] passiveList = {character.passiveSkill, character.abilityT1, character.abilityT2, character.abilityT3,
                                character.basicClass, character.superiorClass, character.weapon, character.armor};

        foreach (PassiveSO so in passiveList)
        {
            if (so == null)
            {
                continue;
            }

            PassiveLogic passive = Utility.GetAbilityBySO(so);
            if (passive != null)
            {
                curCharacterPassive?.Add(passive);
            }
        }
    }

    //스킬 및 패시브 시전자 설정
    private void SetSkillOwner()
    {
        if (curCharacterSkill != null)
        {
            curCharacterSkill.Init(this);
        }

        if (curCharacterPassive != null)
        {
            foreach(PassiveLogic passive in curCharacterPassive)
            {
                passive?.init(this);
            }
        }

        curCharacterBufList = new CharacterBufList(this);
        tempBonusStat = new TempBonusStat();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // 스탯 관련 함수

    public float AtkIncrease
    {
        get
        {
            float increase = 1 * ((float)(100f + character.atkIncrease) / 100f) * curCharacterBufList.GetAdditionalStat().ExtraAtk * tempBonusStat.GetTempStat().ExtraAtk;
            if(increase < 0)
            {
                increase = 1f;
            }

            return increase;
        }
    }

    public float AtkDecrease// 스탯 감소 디버프(합연산, 추가 버프 이후 계산)
    {
        get
        {
            float decrease = curCharacterBufList.GetDecreaseStat().ExtraAtk + tempBonusStat.GetTempDecrease().ExtraAtk;
            if(decrease > 100)
            {
                decrease = 100;
            }
            else if(decrease < 0)
            {
                decrease = 0;
            }

            return (100f - decrease) / 100f;
        }
    }

    public int Attack
    {
        get
        {
            return (int)(((float)character.atk * AtkIncrease) * AtkDecrease);
        }
    }

    public float DefIncrease
    {
        get
        {
            float increase = 1 * ((float)(100 + character.defIncrease) / 100f) * curCharacterBufList.GetAdditionalStat().ExtraDefend * tempBonusStat.GetTempStat().ExtraDefend;
            if (increase < 0)
            {
                increase = 1f;
            }

            return increase;
        }
    }

    public float DefDecrease// 스탯 감소 디버프(합연산, 추가 버프 이후 계산)
    {
        get
        {
            float decrease = curCharacterBufList.GetDecreaseStat().ExtraDefend + tempBonusStat.GetTempDecrease().ExtraDefend;
            if (decrease > 100)
            {
                decrease = 100;
            }
            else if( decrease < 0)
            {
                decrease = 0;
            }

            return (100f - decrease) / 100f;
        }
    }

    public int Defend
    {
        get
        {
            return (int)(((float)character.def * DefIncrease) * DefDecrease);
        }
    }

    public int Mov
    {
        get
        {
            int mov = character.mov + curCharacterBufList.GetAdditionalStat().ExtraMov + tempBonusStat.GetTempStat().ExtraMov - curCharacterBufList.GetDecreaseStat().ExtraMov - tempBonusStat.GetTempDecrease().ExtraMov;
            if(mov < 0)
            {
                mov = 0;
            }

            return mov;
        }
    }

    public int CritRate
    {
        get
        {
            int extraCritRate = character.critRate + curCharacterBufList.GetAdditionalStat().EXCritRate + tempBonusStat.GetTempStat().EXCritRate - curCharacterBufList.GetDecreaseStat().EXCritRate - tempBonusStat.GetTempDecrease().EXCritRate;
            if(extraCritRate < 0)
            {
                extraCritRate = 0;
            }

            return extraCritRate;
        }
    }

    public int CritDMG
    {
        get
        {
            int extraCritDMG = character.critDmg + curCharacterBufList.GetAdditionalStat().EXCritDMG + tempBonusStat.GetTempStat().EXCritDMG - curCharacterBufList.GetDecreaseStat().EXCritDMG - tempBonusStat.GetTempDecrease().EXCritDMG; ;
            if(extraCritDMG < 0)
            {
                extraCritDMG = 0;
            }

            return extraCritDMG;
        }
    }

    public float PenetrateDef//방어력 관통(곱연산)
    {
        get
        {
            float penetrate = 1 * curCharacterBufList.GetAdditionalStat().PenetrateDef + tempBonusStat.GetTempStat().PenetrateDef;
            if(penetrate > 100f)
            {
                penetrate = 100f;
            }
            else if( penetrate < 0f)
            {
                penetrate = 1f;
            }

            return penetrate;
        }
    }

    public float EnhanceDMG
    {
        get
        {
            float enhance = character.EnhancedDmg + curCharacterBufList.GetAdditionalStat().EnhancedDmg + tempBonusStat.GetTempStat().EnhancedDmg;

            return (100 + enhance) / 100;
        }
    }

    public float ReduceDMG
    {
        get
        {
            float reduce = 1 * ((float)(100 + character.ReducedDmg) / 100f) * curCharacterBufList.GetAdditionalStat().ReducedDmg * tempBonusStat.GetTempStat().ReducedDmg;
            if(reduce < 0)
            {
                reduce = 1f;
            }

            return reduce;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    // Update
    private void Update()// 실시간 판정을 위한 Update함수 (예/ 적 뒤에 공간이 있는지 확인, 캐릭터 주위로 버프 등)
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
                passive?.OnUpdate();
        }
        curCharacterSkill.skillAbility?.OnUpdate();
        curCharacterBufList.OnUpdate();
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

    public void MoveTileAndPosition(OverlayTile newTile)// 타일 이동하면서 캐릭터 위치도 이동
    {
        MoveTile(newTile);
        transform.position = newTile.transform.position;
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
                BattleManager.Instance.OnPassCharacter(this, target);
            }

            historyCurrentRound.moveFigure++;

            if (isDead)
            {
                BlockMoving();
                break;
            }

            i++;
        }

        MoveTile(curStandingTile);

        OnEndMoving();

        AnimationController.instance.StartAnimationQueue();
    }

    public void BlockMoving()//이동 막힘
    {
        if (pathedTiles.Count > 0)
        {
            movePath.Clear();

            OverlayTile prevTile = pathedTiles.First();

            foreach (OverlayTile tile in pathedTiles)//지나간 타일에서 비어있는 타일 선택
            {
                historyCurrentRound.moveFigure--;
                if(historyCurrentRound.moveFigure <= 0)
                {
                    historyCurrentRound.moveFigure = 0;
                }

                if (tile.curStandingCharater == null || tile.curStandingCharater == this)
                {
                    curStandingTile = tile;

                    AnimationController.instance.EnqueueBackAnimation(this, prevTile, tile);

                    break;
                }
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
        historyPrevRound = new CharacterHistory(historyCurrentRound);
        historyCurrentRound.ResetHistory();

        foreach(PassiveLogic passive in curCharacterPassive)
        {
                passive?.OnRoundStart();
        }
        curCharacterBufList?.OnRoundStart();

        tempBonusStat.ClearAllStat();
        tempBonusStat.ClearDecreaseStat();
    }

    public virtual void OnStartPlayerTurn()// 턴 시작 시
    {
        isWalking = false;
        isAttacking = false;

        didAttack = false;
        didWalk = false;

        canActing = true;
        canMoveSkil = true;

        ActivateSkill();

        leftWalkRange = Mov;

        foreach(PassiveLogic passive in curCharacterPassive)
        {
                passive?.OnTurnStart();
        }
        curCharacterBufList?.OnTurnStart();
    }

    public void OnPassAlly(CharacterBase allyCharacter)// 아군 위를 지나갔을 때 발동
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnPassAlly(allyCharacter);
        }
        curCharacterBufList?.OnPassAlly(allyCharacter);
    }

    public void OnAllyPassedMe(CharacterBase allyCharacter)// 아군이 이 캐릭터 위를 지나갔을 때 발동
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnAllyPassedMe(allyCharacter);
        }
        curCharacterBufList?.OnAllyPassedMe(allyCharacter);
    }

    public void OnPassEnemy(CharacterBase enemyCharacter)// 적군 위를 지나갔을 때 발동
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnPassEnemy(enemyCharacter);
        }
        curCharacterBufList?.OnPassEnemy(enemyCharacter);
    }

    public void OnEnemyPassesMe(CharacterBase enemyCharacter)// 적군이 이 캐릭터 위를 지나갔을 때 발동
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnEnemyPassesMe(enemyCharacter);
        }
        curCharacterBufList?.OnEnemyPassesMe(enemyCharacter);
    }

    public void OnStartMoving()// 이동 시
    {
        isWalking = true;

        GetAttackTarget();
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnStartMoving();
        }
    }

    public void OnEndMoving()// 이동 끝난 직후
    {
        MapManager.instance.CompleteMove();


        if (!isDead)
        {
            foreach (PassiveLogic passive in curCharacterPassive)
            {
                passive?.OnEndMoving();
            }
        }

        isWalking = false;
        didWalk = true;
        if(character.SO.attackMethod == Constants.AttackMethod.Melee)
        {
            didAttack = true;
        }

        movePath.Clear();
        pathedTiles.Clear();
        leftWalkRange = 0;

        OnEndActing();

        OnEndWalk?.Invoke();

        BattleManager.Instance.CheckWin(null, curStandingTile);
    }

    public void OnEndActing()// 행동이 끝난 뒤
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnEndActing();
        }
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

        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnTurnEnd();
        }
        curCharacterBufList?.OnTurnEnd();
    }

    public void OnRoundEnd()// 턴이 끝날 때
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnRoundEnd();
        }
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
            canActing = false;
        }
    }

    private void CheckActivated()
    {
        if (!canActing && BattleManager.Instance.nowPlayer == player && gameObject.activeInHierarchy)
        {
            characterAnim.DeActivate();
        }
    }

    public bool CheckEnemy(CharacterBase target)// 적인지 확인
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
        BattleManager.Instance.Attack(this, target);
    }

    public void AfterTakeAttacked(CharacterBase enemy)// 공격 받은 이후에
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.AfterTakeAttacked(enemy);
        }
    }

    public void AttackTarget(CharacterBase enemy)// 캐릭터 공격
    {
        target = enemy;
        BattleManager.Instance.DoAttack(this, target);
    }

    public void CounterAttack(CharacterBase enemy)// 반격
    {
        target = enemy;
        BattleManager.Instance.CounterAttack(this, target);
    }

    public void OnStartAttack(CharacterBase enemy)// 공격 시작 시
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnStartAttack(enemy);
        }
        curCharacterBufList?.OnStartAttack(enemy);

        isAttacking = true;
    }

    public void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnAttackSuccess(enemy, damage);
        }
        curCharacterBufList?.OnAttackSuccess(enemy, damage);

        historyCurrentRound.dealDamageFigure += damage.damage;
        if (damage.damage > 0)
        {
            historyCurrentRound.dealDamageCount++;
        }

        if (!historyCurrentRound.gainManaByAttack)
        {
            player.GainMana(5);
            historyCurrentRound.gainManaByAttack = true;
        }
    }

    public void OnEndAttack(CharacterBase enemy)// 공격 종료 시
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnEndAttack(enemy);
        }
        curCharacterBufList?.OnEndAttack(enemy);

        historyCurrentRound.attackCount++;

        EndAttacking();
    }

    private void EndAttacking()// 공격 끝내기
    {
        isAttacking = false;
        if (character.SO.attackMethod == Constants.AttackMethod.Range)
        {
            didAttack = true;

            OnEndActing();
        }
    }

    public void OnTakeAttack(CharacterBase enemy)// 공격 당할 때
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnTakeAttack(enemy);
        }
        curCharacterBufList?.OnTakeAttack(enemy);

    }

    private void GetAttackTarget()// 공격 타겟 가져오기
    {
        List<OverlayTile> temp = new List<OverlayTile>();

        temp = movePath.FindAll(x => x.curStandingCharater != null && x.curStandingCharater.CheckEnemy(this));

        foreach (OverlayTile scale in temp)
        {
            targets.Add(scale.curStandingCharater);
        }
    }

    //---------------------------------------------------------------------------
    // 데미지 관련

    public virtual void OnTakeDamage(ref BattleKeyWords.Damage damage, CharacterBase enemy,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// 공격 받았을 때
    {
        if (damageType != BattleKeyWords.AttackDamageType.Extra)// 추가 피해가 아닌 경우에만 발동
        {
            foreach(PassiveLogic passive in curCharacterPassive)
            {
                passive?.OnTakeDamage(ref damage.damage, enemy, damageType, elementType);
            }
            curCharacterBufList?.OnTakeDamage(ref damage.damage, enemy, damageType, elementType);
        }

        health.TakeDamage(damage);

        AfterTakeDamage(damage.damage, enemy, damageType, elementType);
    }

    public void TakeDamageByInt(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// int만 받아 데미지(주로 버프효과에 의해)
    {
        if (damageType != BattleKeyWords.AttackDamageType.Extra)
        {
            foreach(PassiveLogic passive in curCharacterPassive)
            {
                passive?.OnTakeDamage(ref damage, enemy, damageType, elementType);
            }
            curCharacterBufList?.OnTakeDamage(ref damage, enemy, damageType, elementType);
        }

        health.TakeDamageByInt(damage);

        AfterTakeDamage(damage, enemy, damageType, elementType);
    }

    public void OnTakeHeal(ref BattleKeyWords.Damage heal, CharacterBase skillUser,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// 회복 받았을 때
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnTakeHeal(ref heal.damage, skillUser, damageType, elementType);
        }
        curCharacterBufList?.OnTakeHeal(ref heal.damage, skillUser, damageType, elementType);

        health.HealHealth(heal);

        AfterTakeHeal(heal.damage, skillUser, damageType, elementType);
    }

    public void TakeHealByInt(ref int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// int만 받아 힐(주로 버프효과에 의해)
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnTakeHeal(ref heal, skillUser, damageType, elementType);
        }
        curCharacterBufList?.OnTakeHeal(ref heal, skillUser, damageType, elementType);

        health.HealHealthByInt(heal);

        AfterTakeHeal(heal, skillUser, damageType, elementType);
    }

    public void AfterTakeDamage(int damage, CharacterBase attacker = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// 데미지를 입은 이후에
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.AfterTakeDamage(damage, attacker, damageType, elementType);
        }
        curCharacterBufList?.AfterTakeDamage(damage, attacker, damageType, elementType);

        historyCurrentRound.takeDamageFigure += damage;
        historyCurrentRound.takeDamageCount++;

        if (attacker != null)
        {
            historyCurrentRound.AddAttackEnemy(attacker, damage);
        }
    }

    public void AfterTakeHeal(int heal, CharacterBase skillUser = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType elementType = Constants.ElementType.None)// 힐 받은 이후에
    {
        foreach (PassiveLogic passive in curCharacterPassive)
        {
            passive?.AfterTakeHeal(heal, skillUser, damageType, elementType);
        }
        curCharacterBufList?.AfterTakeHeal(heal, skillUser, damageType, elementType);

        historyCurrentRound.takeHealFigure += heal;
        historyCurrentRound.takeHealCount++;
    }

    //---------------------------------------------------------------------------
    // 스킬 관련

    public void UseSkill()// 스킬 사용
    {
        GetSkillTarget();

        BattleManager.Instance.UseSkill(this, targets);

        historyCurrentRound.useSkillCount++;
    }

    public void GetSkillScale(List<OverlayTile> skillScale)
    {
        this.skillScale = skillScale;
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
                temp = skillScale.FindAll(x => x.curStandingCharater != null && x.curStandingCharater.CheckEnemy(this));
                break;
            case Constants.SkillTargetType.Ally:
                temp = skillScale.FindAll(x => x.curStandingCharater != null && !x.curStandingCharater.CheckEnemy(this));
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

        curCharacterBufList?.OnUseSkill(target);
        curCharacterSkill.skillAbility?.OnUseSkill(target);
        foreach (PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnUseSkill(target);
        }
    }

    public void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 공격 적중 시
    {
        curCharacterBufList?.OnSkillAttackSuccess(target, damage);
        if (curCharacterSkill.skillData.onhit)
        {
            foreach(PassiveLogic passive in curCharacterPassive)
            {
                passive?.OnAttackSuccess(target, damage);
            }
        }
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnSkillAttackSuccess(target, damage);
        }
        curCharacterSkill.skillAbility?.OnSkillAttackSuccess(target, damage);

        historyCurrentRound.dealDamageFigure += damage.damage;
        if (damage.damage > 0)
        {
            historyCurrentRound.dealDamageCount++;
        }
    }

    public void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        curCharacterBufList?.OnSkillHealSuccess(target, heal);

        foreach (PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnSkillHealSuccess(target, heal);
        }
        curCharacterSkill.skillAbility?.OnSkillHealSuccess(target, heal);

        historyCurrentRound.healFigure += heal.damage;
        if (heal.damage > 0)
        {
            historyCurrentRound.healCount++;
        }
    }

    public void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        didUseSkill = true;
        DeActivateSkill();

        curCharacterBufList?.OnEndSkill(target);
        curCharacterSkill.skillAbility?.OnEndSkill(target);
        foreach (PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnEndSkill(target);
        }

        OnEndActing();
    }

    public void ActivateSkill()// 스킬 사용 가능하게 만듬
    {
        didUseSkill = false;
        canSkill = true;
    }

    public void DeActivateSkill()
    {
        canSkill = false;
    }

    //---------------------------------------------------------------------------
    // 캐릭터 사망 시

    public void OnKillEnemy(CharacterBase target, Constants.ElementType elementType = Constants.ElementType.None)
    {
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnKillEnemy(target, elementType);
        }

        player.GainMana(4);
    }

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
        foreach(PassiveLogic passive in curCharacterPassive)
        {
            passive?.OnDieInBattle(killer);
        }

        OnDie();
    }

    public void OnDie()// 사망 시
    {
        if (!onDiePassive)
        {
            player.GainMana(10);

            foreach (PassiveLogic passive in curCharacterPassive)
            {
                passive?.OnDie();
            }
            curCharacterBufList?.OnDie();

            Debug.Log("die");

            BattleManager.Instance.CharacterDie(this);

            onDiePassive = true;
        }
    }

    protected virtual void OnDisable()
    {
        if(curStandingTile == null)
        {
            return;
        }

        curStandingTile.curStandingCharater = null;
        curStandingTile = null;
    }
}
