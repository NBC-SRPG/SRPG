using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Square : SkillScaleBase
{
    public SkillScale_Square(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        GetSurroundingTiles(location, scale);

        return base.GetSkillScale(location, scale);
    }

    private void GetSurroundingTiles(Vector2Int location, int scale)
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();

        OverlayTile startTile = Managers.MapManager.map[location];
        List<OverlayTile> surroundTiles = new List<OverlayTile>();
        int stepCount = 0;

        skillScale.Add(startTile);

        List<OverlayTile> tilesForPreviousStep = new List<OverlayTile>();
        tilesForPreviousStep.Add(startTile);

        while (stepCount < scale)
        {
            surroundTiles.Clear();

            foreach (OverlayTile tile in tilesForPreviousStep)
            {
                surroundTiles.AddRange(Managers.MapManager.GetSurroundingAllTiles(new Vector2Int(tile.gridLocation.x, tile.gridLocation.y)));
            }

            skillScale.AddRange(surroundTiles);
            tilesForPreviousStep = surroundTiles.Distinct().ToList();
            stepCount++;
        }

        skillScale.Distinct().ToList();
    }
}
