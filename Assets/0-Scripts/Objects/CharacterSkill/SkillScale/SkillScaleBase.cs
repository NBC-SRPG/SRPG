using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScaleBase
{
    protected List<OverlayTile> skillScale;
    protected int sclae;
    protected CharacterBase character;

    public SkillScaleBase(CharacterBase character,int scale)
    {
        this.character = character;
        this.sclae = scale;
    }

    public virtual List<OverlayTile> GetSkillScale(Vector2Int location, int scale)
    {

        return skillScale;
    }

    protected Vector2 GetDirection(Vector2Int location)//방향 가져오기
    {
        Vector3 l = new Vector3(location.x, location.y, character.curStandingTile.transform.position.z);
        Vector2 direction = (l - character.curStandingTile.gridLocation).normalized;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (rotZ <= 45f && rotZ > -45f)
        {
            return Vector2.right;
        }
        else if (rotZ <= 135f && rotZ > 45f)
        {
            return Vector2.up;
        }
        else if (rotZ >= -135f && rotZ > 135f)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.down;
        }
    }
}
