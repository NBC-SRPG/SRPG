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
        GetLineTiles(location, scale);

        return base.GetSkillScale(location, scale);
    }

    private void GetLineTiles(Vector2Int location, int scale)
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();
        Vector2 direction = GetDirection(location);

        Vector2Int TileToCheck;

        for(int i = 0; i <= scale; i++)
        {
            TileToCheck = new Vector2Int((int)character.curStandingTile.transform.position.x + ((int)direction.x * i), (int)character.curStandingTile.transform.position.y + ((int)direction.y * i));
            if (Managers.MapManager.map.ContainsKey(TileToCheck))
            {
                if (Managers.MapManager.map[TileToCheck].canClick && !skillScale.Contains(Managers.MapManager.map[TileToCheck]))
                {
                    skillScale.Add(Managers.MapManager.map[TileToCheck]);
                }
            }
        }
    }

    private Vector2 GetDirection(Vector2Int location)
    {
        Vector3 l = new Vector3(location.x, location.y, character.curStandingTile.transform.position.z);
        Vector2 direction = (l - character.curStandingTile.transform.position).normalized;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (rotZ <= 45f && rotZ > -45f)
        {
            return Vector2.right;
        }
        else if(rotZ <= 135f && rotZ > 45f)
        {
            return Vector2.up;
        }
        else if(rotZ >= -135f && rotZ > 135f)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.down;
        }
    }
}
