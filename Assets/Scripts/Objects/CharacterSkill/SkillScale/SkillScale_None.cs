using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScale_None : SkillScaleBase
{
    public SkillScale_None(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale.Add(Managers.MapManager.map[location]);

        return base.GetSkillScale(location, scale);
    }
}
