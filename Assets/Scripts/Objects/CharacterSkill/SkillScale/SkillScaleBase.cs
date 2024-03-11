using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScaleBase
{
    protected List<OverlayTile> skillScale;
    protected int sclae;

    public SkillScaleBase(int scale)
    {
        this.sclae = scale;
    }

    public virtual List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {

        return skillScale;
    }
}
