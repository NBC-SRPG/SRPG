using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleKeyWords;

public class CharacterAI : CharacterBase
{
    [SerializeField] protected EnemyState state;

    protected CharacterBase attractTarget;
    protected CharacterAI ally;

    public bool waiting;

    public event Action Wait;
    public event Action Act;

    protected WaitForSeconds delay = new WaitForSeconds(0.5f);

    protected bool hasSkill = false;

    protected EnemySO enemyData;

    public event Action Disable;

    //-----------------------------------------------------------------------------------------------------------------------
    //override 함수

    public override void InitCharacter(Character charac, GamePlayer player)
    {
        base.InitCharacter(charac, player);

        enemyData = character.enemySO;// 적 데이터를 따로 받아옴

        state = enemyData.startState;

    }

    public override void OnStartPlayerTurn()
    {
        base.OnStartPlayerTurn();

        waiting = false;
    }

    public override void OnTakeDamage(ref BattleKeyWords.Damage damage, CharacterBase enemy, BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None, Constants.ElementType elementType = Constants.ElementType.None)
    {
        base.OnTakeDamage(ref damage, enemy, damageType, elementType);

        if (enemy != null && state == EnemyState.Waiting)
        {
            attractTarget = enemy;
            ChangeState(EnemyState.Chasing);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //--TODO EnemyController에 사망 이벤트
        if (!BattleManager.Instance.gameEnd)
        {
            Disable?.Invoke();
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //실제 행동 함수

    public void StartAI()
    {
        AnimationController.instance.onAnimationEnd += EndActing;

        if (isDead)
        {
            Wait?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;

            return;
        }

        switch (state)
        {
            case EnemyState.Waiting:
                Waiting();
                break;
            case EnemyState.Finding:
                Finding();
                break;
            case EnemyState.Watching:
                Watching();
                break;
            case EnemyState.Chasing:
                Chasing(); 
                break;
        }

    }

    private void AlertEnemy()
    {
        foreach(CharacterAI ally in FindNearAlly())
        {
            if (!ally.isDead)
            {
                ally.GetAlert(this);
            }
        }
    }

    private IEnumerator ChaseEnemy()
    {

        if (hasSkill)// 엘리트 몹 전용
        {

        }

        if (character.SO.attackMethod == Constants.AttackMethod.Melee)// 근접 캐릭터라면
        {
            if (!attractTarget.isDead && canActing)
            {
                yield return delay;

                movePath = FindMeleePath();
                MoveCharacter();
            }
            else
            {
                Wait?.Invoke();

                AnimationController.instance.onAnimationEnd -= EndActing;
            }
        }
        else// 원거리 캐릭터라면
        {
            if (!attractTarget.isDead && !didWalk)
            {
                yield return delay;

                movePath = FindRangePath();
                MoveCharacter();
            }

            if (CheckEnemyInAttackRange() && !didAttack)
            {
                yield return delay;

                SetAttackTarget(attractTarget);
            }
            else
            {
                if(waiting)
                {
                    Wait?.Invoke();

                    AnimationController.instance.onAnimationEnd -= EndActing;
                }

                waiting = true;
            }
        }
    }

    public void EndActing()
    {
        if(attractTarget != null && attractTarget.isDead)// 추적 대상이 죽었다면
        {
            ChangeState(EnemyState.Finding);// 색적 상태로 전환
            return;
        }

        if (waiting)
        {
            Wait?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;
        }

        if (!canActing)// 행동 불가 상태면
        {
            Act?.Invoke();// 행동 끝 콜백

            AnimationController.instance.onAnimationEnd -= EndActing;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //상태에 따른 행동

    private void Finding()// 색적 행동
    {
        if (!canActing)// 행동 불가 상태면
        {
            Act?.Invoke();// 행동 끝 콜백

            AnimationController.instance.onAnimationEnd -= EndActing;
            return;
        }

        CharacterBase nearsetCharacter = FindAttractEnemy();

        if (nearsetCharacter != null)// 적이 공격 범위에 들어왔을 시
        {
            attractTarget = nearsetCharacter;// 해당 적을 목표로 설정

            AlertEnemy();// 주위 아군을 경계상태로 만듬
            ChangeState(EnemyState.Chasing);// 추격 시작
        }
        else// 적이 공격 범위에 없을 시
        {
            //다음 순서로 넘어감
            Wait?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;
            return;
        }
    }

    private void Chasing()// 추격 행동
    {
        if (!canActing)// 행동 불가 상태면
        {
            Act?.Invoke();// 행동 끝 콜백

            AnimationController.instance.onAnimationEnd -= EndActing;
            return;
        }

        CharacterBase nearsetCharacter = FindAttractEnemy();

        if (attractTarget.isDead || (nearsetCharacter != null && attractTarget != nearsetCharacter))// 어그로 끌린 적이 죽었거나 가까운 적이 바뀌었다면
        {
            attractTarget = nearsetCharacter;
        }

        if(attractTarget == null)// 어그로 끌린 적이 없다면
        {
            ChangeState (EnemyState.Finding);// 색적 상태로 전환
            return;
        }

        StartCoroutine(nameof(ChaseEnemy));
    }

    private void Watching()// 경계 행동
    {
        if (!canActing)// 행동 불가 상태면
        {
            Act?.Invoke();// 행동 끝 콜백

            AnimationController.instance.onAnimationEnd -= EndActing;
            return;
        }

        if (ally.isDead)// 추격중인 아군이 죽었다면
        {
            ChangeState(EnemyState.Finding);// 색적상태로 전환
            return;
        }

        CharacterBase nearsetCharacter = FindAttractEnemy();// 이동 전에 적이 있는지 탐색

        if (nearsetCharacter != null)// 적이 있다면
        {
            attractTarget = nearsetCharacter;
            ChangeState(EnemyState.Chasing);// 추격 상태로 전환
            return;
        }

        if (!ally.isDead && !didWalk)
        {
            movePath = FindCoverTile();// 아군 주위 무작위 위치로 이동
            MoveCharacter();
        }

        nearsetCharacter = FindAttractEnemy();// 이동 이후 적이 있는지 탐색

        if(nearsetCharacter != null)// 적이 있다면
        {
            attractTarget = nearsetCharacter;
            ChangeState(EnemyState.Chasing);// 추격 상태로 전환
        }
    }

    private void Waiting()// 대기 행동
    {
        CharacterBase nearsetCharacter = FindAttractEnemy();// 적이 있는지 탐색

        if (nearsetCharacter != null)// 적이 있다면
        {
            attractTarget = nearsetCharacter;
            ChangeState(EnemyState.Chasing);// 추격 상태로 전환
        }
        else // 없다면
        {
            //다음 순서로 넘어감
            Wait?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;
        }
    }

    private void Running()
    {
        CharacterBase nearsetCharacter = FindNearestEnemyByDistance();



    }

    private void Staying()// 비행동
    {
        Wait?.Invoke();
        AnimationController.instance.onAnimationEnd -= EndActing;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //상태 관련 함수들

    protected void ChangeState(EnemyState state)
    {
        this.state = state;

        switch (state)
        {
            case EnemyState.Chasing:
                ally = null;
                Chasing();
                break;
            case EnemyState.Finding:
                ally = null;
                Finding();
                break;
        }
    }

    public void GetAlert(CharacterAI character)
    {
        if(this.state == EnemyState.Finding)// 색적 상태였다면
        {
            ChangeState(EnemyState.Watching);// 경계상태로 전환
        }

        ally = character;
    }

    public void ChaseStart()
    {
        if (character.enemySO.isElite || state == EnemyState.Waiting)
        {
            return;
        }

        attractTarget = FindNearestEnemyByDistance();

        ChangeState(EnemyState.Chasing);
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //길찾기 함수들

    public List<OverlayTile> FindCoverTile()
    {
        int leftWalk = leftWalkRange + 1;

        List<OverlayTile> list = rangeFinder.GetTilesInRange(ally.curStandingTile.grid2DLocation, 3, true);// 아군의 주위 타일 가져옴

        int randomTile = UnityEngine.Random.Range(0, list.Count);

        OverlayTile coverTile = list[randomTile];
        list = pathFinder.FindPath(curStandingTile, coverTile);

        leftWalk -= list.Count;

        if (leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, leftWalkRange);// 이동 횟수에 맞게 경로 자르기
        }

        if (list.Count <= 1)// 이동 가능한 거리가 없다면, 움직이지 않음
        {
            return new List<OverlayTile> { curStandingTile };
        }

        while (!list.Last().CheckCanMove())// 도착지점이 이동 불가 지역일 때
        {
            if (leftWalkRange + 1 - list.Count >= 1)// 걸음 횟수가 남아있다면, 목표 타일 주위로 이동
            {
                foreach (OverlayTile tile in MapManager.instance.GetSurroundingTiles(list.Last().grid2DLocation, true))
                {
                    if (tile.CheckCanMove() && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }

                if (!list.Last().CheckCanMove())// 주위에도 남은 타일이 없다면, 한칸 뒤로
                {
                    list.Remove(list.Last());
                }
            }
            else// 남아있지 않다면, 그 이전 위치로 이동
            {
                list.Remove(list.Last());
            }
        }

        return list;
    }

    public List<CharacterAI> FindNearAlly()
    {
        List<CharacterAI> characterInRange = new List<CharacterAI>();

        foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, 5, false))// 5칸 이내의 아군을 찾음
        {
            if (tile.curStandingCharater != null && !tile.curStandingCharater.CheckEnenmy(this))
            {
                characterInRange.Add((CharacterAI)tile.curStandingCharater);
            }
        }

        return characterInRange;
    }

    public OverlayTile FindNearestTile(List<OverlayTile> tiles, OverlayTile curTile)// 가장 가까운 타일 구하기
    {
        int min = -1;
        OverlayTile nearestTile = null;

        foreach (OverlayTile tile in tiles)
        {
            int distance = pathFinder.FindPath(curTile, tile).Count;

            if ((distance < min || min == -1))
            {
                min = distance;
                nearestTile = tile;
            }
        }

        return nearestTile;
    }

    public CharacterBase FindNearestEnemyByDistance()
    {
        int min = -1;
        CharacterBase nearestCharacter = null;

        foreach (CharacterBase character in BattleManager.Instance.charactersInBattle)
        {
            if (character.playerId == playerId || character.isDead)// 아군 캐릭터이거나 사망한 캐릭터 혹인 이미 탐색한 캐릭터 스킵
            {
                continue;
            }

            int distance = pathFinder.GetManhattenDistance(curStandingTile, character.curStandingTile);

            if ((distance < min || min == -1) && distance != 0 && distance != -1)
            {
                min = distance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    public CharacterBase FindNearestEnemy(OverlayTile curTile, List<CharacterBase> checkedCharacter)// 전체 캐릭터 중에 가장 가까운 적 찾기
    {
        int min = -1;
        CharacterBase nearestCharacter = null;

        foreach(CharacterBase character in BattleManager.Instance.charactersInBattle)
        {
            if (character.playerId == playerId || character.isDead || checkedCharacter.Contains(character))// 아군 캐릭터이거나 사망한 캐릭터 혹인 이미 탐색한 캐릭터 스킵
            {
                continue;
            }

            int distance = pathFinder.FindPath(curTile, character.curStandingTile) != null ? pathFinder.FindPath(curTile, character.curStandingTile).Count : -1 ;

            if ((distance < min || min == -1) && distance != 0 && distance != -1)
            {
                min = distance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    public CharacterBase FindAttractEnemy()// 공격 범위 내에 있는 가장 가까운 적 찾기
    {
        int min = -1;
        CharacterBase nearestCharacter = null;

        List<CharacterBase> charactersInRange = GetCharactersInAttackRange();

        foreach (CharacterBase character in charactersInRange)
        {
            if (!character.CheckEnenmy(this))// 아군 캐릭터면 스킵
            {
                continue;
            }

            int distance = pathFinder.GetManhattenDistance(curStandingTile, character.curStandingTile);

            if ((distance < min || min == -1) && distance != 0)
            {
                min = distance;
                nearestCharacter = character;
            }
        }

        return nearestCharacter;
    }

    public List<CharacterBase> GetCharactersInAttackRange()// 범위 내에 있는 적 찾기
    {
        List<CharacterBase> characterInRange = new List<CharacterBase>();

        if (character.SO.attackMethod == Constants.AttackMethod.Melee)
        {
            foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, leftWalkRange, true))
            {
                if (tile.curStandingCharater != null && tile.curStandingCharater.CheckEnenmy(this))
                {
                    characterInRange.Add(tile.curStandingCharater);
                }
            }
        }
        else
        {
            foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, character.SO.range, false))
            {
                if (tile.curStandingCharater != null && tile.curStandingCharater.CheckEnenmy(this))
                {
                    characterInRange.Add(tile.curStandingCharater);
                }
            }
        }

        return characterInRange;
    }

    private bool CheckEnemyInAttackRange()
    {
        List<OverlayTile> range = rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, character.SO.range, false);

        if (range.Contains(attractTarget.curStandingTile))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<OverlayTile> FindMeleePath()// 근거리 캐릭터 이동 경로 찾기
    {
        int leftWalk = leftWalkRange + 1;
        List<CharacterBase> checkCharacter = new List<CharacterBase>();

        List<OverlayTile> list = new List<OverlayTile> { curStandingTile };
        list.AddRange(pathFinder.FindPath(curStandingTile, attractTarget.curStandingTile));// 현재 어그로 끌린 캐릭터를 향하는 경로

        checkCharacter.Add(attractTarget);

        leftWalk -= list.Count;

        while(leftWalk > 0)//이동 횟수가 아직 남아있다면,
        {
            CharacterBase nearestCharacter = FindNearestEnemy(list.Last(), checkCharacter);
            checkCharacter.Add(nearestCharacter);

            if (nearestCharacter == null)// 다른 적이 없다면
            {
                foreach (OverlayTile tile in MapManager.instance.GetSurroundingTiles(list.Last().grid2DLocation, true))// 그 주위로 이동
                {
                    if (tile.CheckCanMove() && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }
                break;
            }

            List<OverlayTile> anotherPath = pathFinder.FindPathAnother(list.Last(), nearestCharacter.curStandingTile, list);

            if (anotherPath == null)
            {
                break;
            }
            list.AddRange(anotherPath);//다음으로 가까운 적을 향해 이동

            leftWalk = leftWalkRange + 1;
            leftWalk -= list.Count;
        }

        if(leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, leftWalkRange);// 이동 횟수에 맞게 경로 자르기
        }

        if(list.Count <= 1)// 이동 가능한 거리가 없다면, 움직이지 않음
        {
            return new List<OverlayTile> { curStandingTile };
        }

        while(!list.Last().CheckCanMove())// 도착지점이 이동 불가 지역일 때
        {
            if (leftWalkRange + 1 - list.Count >= 1)// 걸음 횟수가 남아있다면, 목표 타일 주위로 이동
            {
                foreach (OverlayTile tile in MapManager.instance.GetSurroundingTiles(list.Last().grid2DLocation, true))
                {
                    if (tile.CheckCanMove() && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }

                if(!list.Last().CheckCanMove())// 주위에도 남은 타일이 없다면, 한칸 뒤로
                {
                    list.Remove(list.Last());
                }
            }
            else// 남아있지 않다면, 그 이전 위치로 이동
            {
                list.Remove(list.Last());
            }
        }

        return list;
    }

    public List<OverlayTile> FindRangePath()// 원거리 이동 경로 찾기
    {
        int leftWalk = leftWalkRange + 1;

        List<OverlayTile> kiteRange = rangeFinder.GetTilesInRange(attractTarget.curStandingTile.grid2DLocation, character.SO.range, false);// 목표 대상으로 부터 공격 사거리가 닿는 부분
        List<OverlayTile> range = rangeFinder.GetTilesInRange(attractTarget.curStandingTile.grid2DLocation, character.SO.range - 1, false);

        kiteRange = kiteRange.Except(range).ToList();// 끝 사거리만 가져옴

        kiteRange = kiteRange.FindAll(x => x.canClick);// 이동 가능한 타일 선택

        OverlayTile nearestTile = FindNearestTile(kiteRange, curStandingTile);// 그 중에서 가장 가까운 타일 찾기

        while(nearestTile.curStandingCharater != null)
        {
            kiteRange.Remove(nearestTile);

            nearestTile = FindNearestTile(kiteRange, curStandingTile);
        }

        List<OverlayTile> list = pathFinder.FindPath(curStandingTile, nearestTile);

        leftWalk -= list.Count;

        if (leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, leftWalkRange);// 이동 횟수에 맞게 경로 자르기
        }

        if(list.Count == 0)
        {
            return new List<OverlayTile>() { curStandingTile };
        }

        while (!list.Last().CheckCanMove())// 도착지점이 이동 불가 지역일 때
        {
            Debug.Log("there is");

            if (Mov + 1 - list.Count >= 1)// 걸음 횟수가 남아있다면, 목표 타일 주위로 이동
            {
                foreach (OverlayTile tile in MapManager.instance.GetSurroundingTiles(list.Last().grid2DLocation, true))
                {
                    if (tile.CheckCanMove() && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }

                if (!list.Last().CheckCanMove())// 주위에도 남은 타일이 없다면, 한칸 뒤로
                {
                    list.Remove(list.Last());
                }
            }
            else// 남아있지 않다면, 그 이전 위치로 이동
            {
                list.Remove(list.Last());
            }
        }

        return list;
    }

}
