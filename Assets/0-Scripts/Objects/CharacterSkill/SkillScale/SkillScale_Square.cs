using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Square : SkillScaleBase
{
    RangeFinder rangeFinder;

    public SkillScale_Square(CharacterBase character, int scale) : base(character, scale)
    {
        rangeFinder = new RangeFinder();
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        skillScale = rangeFinder.GetTilesInRangeAll(location, scale, false);//사각형 범위 가져오기

        return base.GetSkillScale(location, scale);
    }

}
