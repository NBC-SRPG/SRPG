using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    [SerializeField] private GameObject defaultTile;
    [SerializeField] private GameObject movepathTile;
    [SerializeField] private GameObject moveTile;
    [SerializeField] private GameObject attackRangeTile;

    [HideInInspector] public bool canClick;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public CharacterBase curStandingCharater;

    private void Start()
    {
        HideTile();
    }

    public void ResetTile()
    {
        if (canClick)
        {
            defaultTile.SetActive(true);
            movepathTile.SetActive(false);
            moveTile.SetActive(false);
            attackRangeTile.SetActive(false);
        }
    }

    public void ShowTile()
    {
        if (canClick)
        {
            defaultTile.SetActive(true);
        }
    }

    public void ShowAsMove()
    {
        if (canClick)
        {
            moveTile.SetActive(true);
        }
    }

    public void ShowAsScale()
    {
        if (canClick)
        {
            moveTile.SetActive(false);
            attackRangeTile.SetActive(false);
            movepathTile.SetActive(true);
        }
    }

    public void ShowAsAttack()
    {
        if (canClick)
        {
            attackRangeTile.SetActive(true);
        }
    }

    public void HideTile()
    {
        defaultTile.SetActive(false);
        movepathTile.SetActive(false);
        moveTile.SetActive(false);
        attackRangeTile.SetActive(false);
    }
}
