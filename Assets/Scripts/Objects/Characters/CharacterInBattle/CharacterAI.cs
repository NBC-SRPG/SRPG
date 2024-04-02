using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    protected PathFinder pathFinder;
    protected RangeFinder rangeFinder;

    protected CharacterBase attractTarget;

    public bool waiting;

    public event Action Waiting;
    public event Action Acting;

    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    //-----------------------------------------------------------------------------------------------------------------------
    //override 함수

    public override void InitCharacter(Character charac, string id)
    {
        base.InitCharacter(charac, id);

        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
    }

    public override void OnStartPlayerTurn()
    {
        base.OnStartPlayerTurn();

        waiting = false;
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //실제 행동 함수

    public void StartAI()
    {
        CharacterBase nearsetCharacter = FindAttractEnemy();// 공격 가능 범위에 적이 있는지 탐색

        AnimationController.instance.onAnimationEnd += EndActing;

        if(attractTarget == null || attractTarget.isDead || (nearsetCharacter != null && attractTarget != nearsetCharacter))// 어그로에 끌린 적이 없거나 어그로 끌린 적이 죽었거나 가까운 적이 바뀌었다면
        {
            attractTarget = nearsetCharacter;
        }

        if (attractTarget == null || !canActing)// 적이 없거나 현재 행동 불가 상태라면
        {
            //다음 순서로 넘어감
            Waiting?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;
            return;
        }

        StartCoroutine(nameof(DoActing));
    }

    private IEnumerator DoActing()
    {
        if (character.CharacterAttackType == Constants.AttackType.Melee)
        {
            yield return delay;

            movePath = FindMeleePath();
            MoveCharacter();
        }
        else
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
                Waiting?.Invoke();

                AnimationController.instance.onAnimationEnd -= EndActing;
            }
        }
    }

    public void EndActing()
    {
        if (!canActing)
        {
            Acting?.Invoke();

            AnimationController.instance.onAnimationEnd -= EndActing;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //길찾기 함수들

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

    public CharacterBase FindNearestEnemy(OverlayTile curTile, List<CharacterBase> checkedCharacter)// 전체 캐릭터 중에 가장 가까운 적 찾기
    {
        int min = -1;
        CharacterBase nearestCharacter = null;

        foreach(CharacterBase character in Managers.BattleManager.charactersInBattle)
        {
            if (character.playerId == playerId || character.isDead || checkedCharacter.Contains(character))// 아군 캐릭터이거나 사망한 캐릭터 혹인 이미 탐색한 캐릭터 스킵
            {
                continue;
            }

            int distance = pathFinder.FindPath(curTile, character.curStandingTile).Count;

            if ((distance < min || min == -1) && distance != 0)
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

        if (character.CharacterAttackType == Constants.AttackType.Melee)
        {
            foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, Mov, true))
            {
                if (tile.curStandingCharater != null && tile.curStandingCharater.CheckEnenmy(this))
                {
                    characterInRange.Add(tile.curStandingCharater);
                }
            }
        }
        else
        {
            foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, character.characterData.atk_range, false))
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
        List<OverlayTile> range = rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, character.characterData.atk_range, false);

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
        int leftWalk = Mov;
        List<CharacterBase> checkCharacter = new List<CharacterBase>();

        List<OverlayTile> list = pathFinder.FindPath(curStandingTile, attractTarget.curStandingTile);// 현재 어그로 끌린 캐릭터를 향하는 경로
        checkCharacter.Add(attractTarget);

        leftWalk -= list.Count;

        while(leftWalk > 0)//이동 횟수가 아직 남아있다면,
        {
            CharacterBase nearestCharacter = FindNearestEnemy(list.Last(), checkCharacter);
            checkCharacter.Add(nearestCharacter);

            if (nearestCharacter == null)// 다른 적이 없다면
            {
                foreach (OverlayTile tile in Managers.MapManager.GetSurroundingTiles(list.Last().grid2DLocation, true))// 그 주위로 이동
                {
                    if (tile.curStandingCharater == null && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }
                break;
            }

            list.AddRange(pathFinder.FindPath(list.Last(), nearestCharacter.curStandingTile));//다음으로 가까운 적을 향해 이동

            leftWalk = Mov;
            leftWalk -= list.Count;
        }

        if(leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, Mov);// 이동 횟수에 맞게 경로 자르기
        }

        if(list.Count <= 1)// 이동 가능한 거리가 없다면, 움직이지 않음
        {
            return new List<OverlayTile> { curStandingTile };
        }

        while(list.Last().curStandingCharater != null)// 도착지점에 캐릭터가 있을 때
        {
            if (Mov - list.Count >= 1)// 걸음 횟수가 남아있다면, 목표 타일 주위로 이동
            {
                foreach (OverlayTile tile in Managers.MapManager.GetSurroundingTiles(list.Last().grid2DLocation, true))
                {
                    if (tile.curStandingCharater == null && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }

                if(list.Last().curStandingCharater != null)// 주위에도 남은 타일이 없다면, 한칸 뒤로
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
        int leftWalk = Mov;

        List<OverlayTile> kiteRange = rangeFinder.GetTilesInRange(attractTarget.curStandingTile.grid2DLocation, character.characterData.atk_range, false);// 목표 대상으로 부터 공격 사거리가 닿는 부분
        List<OverlayTile> range = rangeFinder.GetTilesInRange(attractTarget.curStandingTile.grid2DLocation, character.characterData.atk_range - 1, false);

        kiteRange = kiteRange.Except(range).ToList();// 끝 사거리만 가져옴

        kiteRange = kiteRange.FindAll(x => x.canClick);// 이동 가능한 타일 선택

        OverlayTile nearestTile = FindNearestTile(kiteRange, curStandingTile);// 그 중에서 가장 가까운 타일 찾기

        while(nearestTile.curStandingCharater != null)
        {
            kiteRange.Remove(nearestTile);

            nearestTile = FindNearestTile(kiteRange, curStandingTile);
        }

        List<OverlayTile> list = pathFinder.FindPath(curStandingTile, nearestTile);

        Debug.Log(list.Count);

        leftWalk -= list.Count;

        if (leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, Mov);// 이동 횟수에 맞게 경로 자르기
        }

        if(list.Count == 0)
        {
            return new List<OverlayTile>() { curStandingTile };
        }

        while (list.Last().curStandingCharater != null)// 도착지점에 캐릭터가 있을 때
        {
            Debug.Log("there is");

            if (Mov - list.Count >= 1)// 걸음 횟수가 남아있다면, 목표 타일 주위로 이동
            {
                foreach (OverlayTile tile in Managers.MapManager.GetSurroundingTiles(list.Last().grid2DLocation, true))
                {
                    if (tile.curStandingCharater == null && !list.Contains(tile))
                    {
                        list.Add(tile);
                        break;
                    }
                }

                if (list.Last().curStandingCharater != null)// 주위에도 남은 타일이 없다면, 한칸 뒤로
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
