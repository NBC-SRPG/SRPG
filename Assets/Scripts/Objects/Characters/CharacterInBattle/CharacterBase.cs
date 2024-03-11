using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Character character;

    public OverlayTile curStandingTile;
    public int leftWalkRange;
    [HideInInspector] public bool isDead;

    public List<OverlayTile> movePath = new List<OverlayTile>();

    private void Start()
    {
        Managers.MapManager.OnCompleteMove += CheckCurTile;
        Instantiate(character, gameObject.transform);
        character.InitSkills();
        character.InitPassive();
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
