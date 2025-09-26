using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScale_Line : SkillScaleBase
{
    public SkillScale_Line(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        skillScale = GetLineTiles(location, scale);

        return base.GetSkillScale(location, scale);
    }

    private List<OverlayTile> GetLineTiles(Vector2Int location, int scale)//직선 형태 스킬 범위 가져오기
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();
        Vector2 direction = GetDirection(location);

        Vector2Int TileToCheck;

        for(int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int((int)character.curStandingTile.grid2DLocation.x + ((int)direction.x * i), (int)character.curStandingTile.grid2DLocation.y + ((int)direction.y * i));
            if (MapManager.instance.map.ContainsKey(TileToCheck))
            {
                if (MapManager.instance.map[TileToCheck].canClick && !skillScale.Contains(MapManager.instance.map[TileToCheck]))
                {
                    skillScale.Add(MapManager.instance.map[TileToCheck]);
                }
            }
        }

        return skillScale;
    }

}
