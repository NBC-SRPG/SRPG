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
        skillScale = character.rangeFinder.GetTilesInRange(location, scale, false);

        return base.GetSkillScale(location, scale);
    }

}
