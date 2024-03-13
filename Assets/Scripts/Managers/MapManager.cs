using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager
{
    private Vector2Int[] direction = new Vector2Int[] 
    {
        new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1),
    };

    public Dictionary<Vector2Int, OverlayTile> map = new Dictionary<Vector2Int, OverlayTile>();
    public Dictionary<int, List<Vector2Int>> startTiles = new Dictionary<int, List<Vector2Int>>();
    //public List<Vector2Int> startTile = new List<Vector2Int>();

    public event Action OnCompleteMove;

    public void Init()// 모든 타일 초기화
    {
        map.Clear();
        startTiles.Clear();
    }

    public void CompleteMove()// 이동 완료 시 이벤트
    {
        OnCompleteMove?.Invoke();
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile, bool clickable = false)//4 방향 타일 가져오기
    {
        List<OverlayTile> surroundingTiles = new List<OverlayTile>();
        Vector2Int TileToCheck;

        for (int i = 0; i < 4; i++)
        {
            TileToCheck = new Vector2Int(originTile.x + direction[i].x, originTile.y + direction[i].y);
            if (map.ContainsKey(TileToCheck))
            {
                if (clickable)
                {
                    if (map[TileToCheck].canClick)
                    {
                        surroundingTiles.Add(map[TileToCheck]);
                    }
                }
                else
                {
                    surroundingTiles.Add(map[TileToCheck]);
                }
            }
        }

        return surroundingTiles;
    }

    public List<OverlayTile> GetSurroundingAllTiles(Vector2Int originTile, bool clickable = false)//8 방향 타일 가져오기
    {
        List<OverlayTile> surroundingTiles = new List<OverlayTile>();
        Vector2Int TileToCheck;

        for (int i = 0; i < direction.Length; i++)
        {
            TileToCheck = new Vector2Int(originTile.x + direction[i].x, originTile.y + direction[i].y);
            if (map.ContainsKey(TileToCheck))
            {
                if (clickable)
                {
                    if (map[TileToCheck].canClick)
                    {
                        surroundingTiles.Add(map[TileToCheck]);
                    }
                }
                else
                {
                    surroundingTiles.Add(map[TileToCheck]);
                }
            }
        }

        return surroundingTiles;
    }
}
