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

    public List<OverlayTile> GetSurroundingTiles(Vector2Int location, int scale)
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();
        Vector2Int TileToCheck;

        for (int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int(location.x + i, location.y);
            if (Managers.MapManager.map.ContainsKey(TileToCheck))
            {
                if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                    {
                        skillScale.Add(Managers.MapManager.map[TileToCheck]);
                    }
            }
        }

        for (int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int(location.x - i, location.y);
            if (Managers.MapManager.map.ContainsKey(TileToCheck))
            {
                if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                {
                    skillScale.Add(Managers.MapManager.map[TileToCheck]);
                }
            }
        }

        for (int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int(location.x, location.y + i);
            if (Managers.MapManager.map.ContainsKey(TileToCheck))
            {
                if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                {
                    skillScale.Add(Managers.MapManager.map[TileToCheck]);
                }
            }
        }

        for (int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int(location.x, location.y - i);
            if (Managers.MapManager.map.ContainsKey(TileToCheck))
            {
                if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                {
                    skillScale.Add(Managers.MapManager.map[TileToCheck]);
                }
            }
        }

        return skillScale;
    }
}
