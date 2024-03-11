using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public OverlayTile curStandingTile;
    public int walkRange;

    private void Start()
    {
        Managers.MapManager.OnCompleteMove += CheckCurTile;
    }

    public void CheckCurTile()
    {
        if (curStandingTile.curStandingCharater == null)
        {
            curStandingTile.curStandingCharater = this;
        }
    }

    public void MoveTile(OverlayTile newTile)
    {
        curStandingTile.curStandingCharater = null;
        curStandingTile = newTile;
        curStandingTile.curStandingCharater = this;
    }
}
