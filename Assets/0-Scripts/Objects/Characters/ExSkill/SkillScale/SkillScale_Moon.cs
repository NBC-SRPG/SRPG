using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillScale_Moon : SkillScaleBase
{
    public SkillScale_Moon(CharacterBase character, int scale) : base(character, scale)
    {
    }

    public override List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {
        skillScale = new List<OverlayTile>();
        skillScale = GetMoonTiles(location, scale);

        return base.GetSkillScale(location, scale);
    }

    private List<OverlayTile> GetMoonTiles(Vector2Int location, int scale)//초승달 형태 스킬 범위 가져오기
    {
        List<OverlayTile> skillScale = new List<OverlayTile>();
        Vector2 direction = GetDirection(location);

        if(location == character.curStandingTile.grid2DLocation)
        {
            direction = Vector2.right;
        }

        skillScale = character.rangeFinder.GetTilesInRangeAll(character.curStandingTile.grid2DLocation, scale, false);

        int index = 1;
        while(index <= sclae)
        {
            if(direction.x != 0)
            {
                skillScale = skillScale.Except(skillScale.FindAll(x => x.grid2DLocation.x == character.curStandingTile.grid2DLocation.x + (index * -direction.x))).ToList();
            }
            else if(direction.y != 0)
            {
                skillScale = skillScale.Except(skillScale.FindAll(x => x.grid2DLocation.y == character.curStandingTile.grid2DLocation.y + (index * -direction.y))).ToList();
            }

            index++;
        }

        return skillScale;
    }
}
