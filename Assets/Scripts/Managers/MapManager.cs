using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public Dictionary<Vector2Int, OverlayTile> map = new Dictionary<Vector2Int, OverlayTile>();
    public List<Vector2Int> startTile = new List<Vector2Int>();

    public event Action OnCompleteMove;

    public void Init()
    {
        map.Clear();
        startTile.Clear();
    }

    public void CompleteMove()
    {
        OnCompleteMove?.Invoke();
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile, bool clickable = false)
    {
        List<OverlayTile> surroundingTiles = new List<OverlayTile>();

        Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
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

        TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
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

        TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
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

        TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
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

        return surroundingTiles;
    }
}
