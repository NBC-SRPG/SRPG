using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Rhombus : SkillScaleBase
{
    RangeFinder rangeFinder;

    public SkillScale_Rhombus(CharacterBase character, int scale) : base(character, scale)
    {
        rangeFinder = new RangeFinder();
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        skillScale = rangeFinder.GetTilesInRange(location, scale, false);

        return base.GetSkillScale(location, scale);
    }

}
