using System.Collections;
using System.Collections.Generic;
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
}
