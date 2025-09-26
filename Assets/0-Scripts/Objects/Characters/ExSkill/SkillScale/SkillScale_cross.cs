using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Cross : SkillScaleBase
{
    public SkillScale_Cross(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        skillScale = GetSurroundingTiles(location, scale);

        return base.GetSkillScale(location, scale);
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int location, int scale)//십자형 범위 가져오기
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();
        Vector2Int TileToCheck;

        for (int i = 0; i <= scale; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                TileToCheck = new Vector2Int(location.x + (MapManager.instance.direction[j].x * i), location.y + (MapManager.instance.direction[j].y * i));
                if (MapManager.instance.map.ContainsKey(TileToCheck))
                {
                    if (MapManager.instance.map[TileToCheck].canClick && !skillScale.Contains(MapManager.instance.map[TileToCheck]))
                    {
                        skillScale.Add(MapManager.instance.map[TileToCheck]);
                    }
                }
            }
        }

        return skillScale;
    }
}
