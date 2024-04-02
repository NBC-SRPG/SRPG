using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAI : CharacterBase
{
    protected PathFinder pathFinder;
    protected RangeFinder rangeFinder;

    protected CharacterBase attractTarget;

    public override void InitCharacter(Character charac, string id)
    {
        base.InitCharacter(charac, id);

        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
    }

    //-----------------------------------------------------------------------------------------------------------------------
    //실제 행동 함수



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

    public CharacterBase FindNearestEnemy(OverlayTile curTile)// 전체 캐릭터 중에 가장 가까운 적 찾기
    {
        int min = -1;
        CharacterBase nearestCharacter = null;

        foreach(KeyValuePair<string, List<CharacterBase>> list in Managers.BattleManager.charactersAsTeam)
        {
            foreach(CharacterBase character in list.Value)
            {
                if(character.playerId == playerId)// 아군 캐릭터면 스킵
                {
                    continue;
                }

                int distance = pathFinder.FindPath(curTile, character.curStandingTile).Count;

                if((distance < min || min == -1) && distance != 0)
                {
                    min = distance;
                    nearestCharacter = character;
                }
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
            foreach (OverlayTile tile in rangeFinder.GetTilesInRange(curStandingTile.grid2DLocation, Mov, false))
            {
                if (tile.curStandingCharater != null && tile.curStandingCharater.CheckEnenmy(this))
                {
                    characterInRange.Add(tile.curStandingCharater);
                }
            }
        }

        return characterInRange;
    }

    public List<OverlayTile> FindMeleePath()// 근거리 캐릭터 이동 경로 찾기
    {
        int leftWalk = leftWalkRange;

        List<OverlayTile> list = pathFinder.FindPath(curStandingTile, attractTarget.curStandingTile);// 현재 어그로 끌린 캐릭터를 향하는 경로

        leftWalk -= list.Count;

        while(leftWalk > 0)//이동 횟수가 아직 남아있다면,
        {
            CharacterBase nearestCharacter = FindNearestEnemy(list.Last());

            list = pathFinder.FindPath(list.Last(), nearestCharacter.curStandingTile);//다음으로 가까운 적을 향해 이동
        }

        if(leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, Mov);// 이동 횟수에 맞게 경로 자르기
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
        int leftWalk = leftWalkRange;

        List<OverlayTile> kiteRange = rangeFinder.GetTilesInRange(attractTarget.curStandingTile.grid2DLocation, character.characterData.atk_range, false);// 목표 대상으로 부터 공격 사거리가 닿는 부분
        kiteRange = kiteRange.GetRange(kiteRange.Count - (character.characterData.atk_range * 4), character.characterData.atk_range * 4);// 사거리의 끝부분만 가져옴

        OverlayTile nearestTile = FindNearestTile(kiteRange, curStandingTile);// 그 중에서 가장 가까운 타일 찾기

        List<OverlayTile> list = pathFinder.FindPath(curStandingTile, nearestTile);

        leftWalk -= list.Count;

        if (leftWalk <= 0)// 현재 경로가 이동 횟수를 넘어갔다면
        {
            list = list.GetRange(0, Mov);// 이동 횟수에 맞게 경로 자르기
        }

        while (list.Last().curStandingCharater != null)// 도착지점에 캐릭터가 있을 때
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
            }
            else// 남아있지 않다면, 그 이전 위치로 이동
            {
                list.Remove(list.Last());
            }
        }

        return list;
    }
}
