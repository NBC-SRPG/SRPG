using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInRange(Vector2Int location, int range, bool isMove)
    {
        OverlayTile startTile = MapManager.instance.map[location];
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
                foreach(OverlayTile t in MapManager.instance.GetSurroundingTiles(tile.grid2DLocation, isMove))
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
        OverlayTile startTile = MapManager.instance.map[location];
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
                foreach (OverlayTile t in MapManager.instance.GetSurroundingAllTiles(tile.grid2DLocation, isMove))
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

    public Vector2 GetDirection(Vector2Int location, CharacterBase curCharacter)//방향 가져오기
    {
        Vector3 l = new Vector3(location.x, location.y, curCharacter.curStandingTile.transform.position.z);
        Vector2 direction = (l - curCharacter.curStandingTile.gridLocation).normalized;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (rotZ <= 45f && rotZ > -45f)
        {
            return Vector2.right;
        }
        else if (rotZ <= 135f && rotZ > 45f)
        {
            return Vector2.up;
        }
        else if (rotZ >= -135f && rotZ > 135f)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.down;
        }
    }

    public OverlayTile GetBackOfCharacter(CharacterBase curCharacter, CharacterBase target)
    {
        OverlayTile back = null;
        Vector2Int targetGrid = target.curStandingTile.grid2DLocation;

        Vector2 direction = GetDirection(targetGrid, curCharacter);

        Vector2Int backOfTarget = new Vector2Int(targetGrid.x + (int)direction.x, targetGrid.y + (int)direction.y);

        if(MapManager.instance.map.ContainsKey(backOfTarget))
        {
            back = MapManager.instance.map[backOfTarget];
        }

        return back;
    }
}
