using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Rhombus : SkillScaleBase
{
    public SkillScale_Rhombus(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        GetTilesInRange(location, scale);

        return base.GetSkillScale(location, scale);
    }
    public void GetTilesInRange(Vector2Int location, int range)
    {
        OverlayTile startTile = Managers.MapManager.map[location];

        List<OverlayTile> surroundTiles = new List<OverlayTile>();
        int stepCount = 0;

        skillScale.Add(startTile);

        List<OverlayTile> tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            surroundTiles.Clear();

            foreach (OverlayTile tile in tilesForPreviousStep)
            {
                surroundTiles.AddRange(Managers.MapManager.GetSurroundingTiles(new Vector2Int(tile.gridLocation.x, tile.gridLocation.y)));
            }

            skillScale.AddRange(surroundTiles);
            tilesForPreviousStep = surroundTiles.Distinct().ToList();
            stepCount++;
        }

        skillScale.Distinct().ToList();
    }
}
