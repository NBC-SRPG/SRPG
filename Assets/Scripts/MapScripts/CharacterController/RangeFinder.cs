using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInRange(Vector2Int location, int range, bool isMove)
    {
        OverlayTile startTile = Managers.MapManager.map[location];
        List<OverlayTile> inRangeTile = new List<OverlayTile>();
        List<OverlayTile> surroundTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTile.Add(startTile);

        List<OverlayTile> tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            surroundTiles.Clear();

            foreach (OverlayTile tile in tilesForPreviousStep)
            {
                foreach(OverlayTile t in Managers.MapManager.GetSurroundingTiles(tile.grid2DLocation, isMove))
                {
                    if (!inRangeTile.Contains(t))
                    {
                        surroundTiles.Add(t);
                    }
                }
            }

            inRangeTile.AddRange(surroundTiles);
            tilesForPreviousStep = surroundTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    }

    public List<OverlayTile> GetTilesInRangeAll(Vector2Int location, int range, bool isMove)
    {
        OverlayTile startTile = Managers.MapManager.map[location];
        List<OverlayTile> inRangeTile = new List<OverlayTile>();
        List<OverlayTile> surroundTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTile.Add(startTile);

        List<OverlayTile> tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            surroundTiles.Clear();

            foreach (OverlayTile tile in tilesForPreviousStep)
            {
                foreach (OverlayTile t in Managers.MapManager.GetSurroundingAllTiles(tile.grid2DLocation, isMove))
                {
                    if (!inRangeTile.Contains(t))
                    {
                        surroundTiles.Add(t);
                    }
                }
            }

            inRangeTile.AddRange(surroundTiles);
            tilesForPreviousStep = surroundTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    }

}
