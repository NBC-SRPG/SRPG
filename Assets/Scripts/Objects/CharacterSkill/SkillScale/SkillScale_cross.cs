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
        GetSurroundingTiles(location, scale);

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
                TileToCheck = new Vector2Int(location.x + (Managers.MapManager.direction[j].x * i), location.y + (Managers.MapManager.direction[j].y * i));
                if (Managers.MapManager.map.ContainsKey(TileToCheck))
                {
                    if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                    {
                        skillScale.Add(Managers.MapManager.map[TileToCheck]);
                    }
                }
            }
        }

        return skillScale;
    }
}
