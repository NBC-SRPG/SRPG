using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInRange(Vector2Int location, int range, bool isformove)//���� �������� bool�� true �� �̵� ���� Ÿ������ üũ��
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
                surroundTiles.AddRange(Managers.MapManager.GetSurroundingTiles(new Vector2Int(tile.gridLocation.x, tile.gridLocation.y), isformove));
            }

            inRangeTile.AddRange(surroundTiles);
            tilesForPreviousStep = surroundTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    }


}
