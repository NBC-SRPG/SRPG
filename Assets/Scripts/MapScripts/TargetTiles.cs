using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTiles : MonoBehaviour
{
    [SerializeField] private GameObject curSelectedTile;
    [SerializeField] private GameObject curTargetTile;

    public void ShowSelectedTile(CharacterBase curCharacter)
    {
        curSelectedTile.SetActive(curCharacter);
        if (curCharacter && curCharacter.curStandingTile)
        {
            curSelectedTile.transform.position = curCharacter.curStandingTile.transform.position;
        }
    }

    public void ShowTargetTile(CharacterBase curTarget)
    {
        curTargetTile.SetActive(curTarget);
        if (curTarget && curTarget.curStandingTile)
        {
            curTargetTile.transform.position = curTarget.curStandingTile.transform.position;
        }
    }
}
