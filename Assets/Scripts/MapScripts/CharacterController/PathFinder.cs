using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<OverlayTile> MakePath(OverlayTile newTile, List<OverlayTile> prevTiles)//다음 이동 가능한 위치 가져옴
    {
        OverlayTile tile = newTile;

        List<OverlayTile> surroundTiles = new List<OverlayTile>();

        foreach (OverlayTile path in Managers.MapManager.GetSurroundingTiles(tile.grid2DLocation, true))
        {
            if (!prevTiles.Contains(path))
            {
                surroundTiles.Add(path);
            }
        }

        return surroundTiles;
    }

    public List<OverlayTile> FindPath(OverlayTile startTile, OverlayTile targetTile)
    {
        List<OverlayTile> openList = new List<OverlayTile>();// 탐색할 타일
        List<OverlayTile> closedList = new List<OverlayTile>();// 탐색한 타일

        openList.Add(startTile);

        while(openList.Count > 0)
        {
            OverlayTile curTile = openList.OrderBy(x => x.F).First();// F값이 가장 작은 타일 가져옴

            openList.Remove(curTile);
            closedList.Add(curTile);

            if(curTile == targetTile)
            {
                return GetPathList(startTile, targetTile);
            }

            List<OverlayTile> surroundTile = Managers.MapManager.GetSurroundingTiles(curTile.grid2DLocation, true);

            foreach(OverlayTile tile in surroundTile)
            {
                if (closedList.Contains(tile))// 이미 탐색한 타일이면
                {
                    continue;
                }

                tile.G = curTile.G + 1;// 걸음 횟수 증가
                tile.H = GetManhattenDistance(targetTile, tile);// 현재 타일에서 목표까지 거리

                tile.prevTile = curTile;

                if(!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return null;

    }

    private List<OverlayTile> GetPathList(OverlayTile startTile, OverlayTile targetTile)
    {
        List<OverlayTile> pathList = new List<OverlayTile> ();

        OverlayTile curTile = targetTile;

        while(curTile != startTile)// 역순으로 타일 리스트에 삽입
        {
            pathList.Add(curTile);
            curTile = curTile.prevTile;
        }

        pathList.Reverse();// 역순으로 들어간 리스트 뒤집음

        return pathList;
    }

    public int GetManhattenDistance(OverlayTile start, OverlayTile target)// 맨허튼 거리 구하기
    {
        int xDistance = Mathf.Abs(start.gridLocation.x - target.gridLocation.x);
        int yDistance = Mathf.Abs(start.gridLocation.y - target.gridLocation.y);

        return xDistance + yDistance;
    }
}
