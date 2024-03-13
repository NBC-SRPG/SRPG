using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    [SerializeField] private GameObject defaultTile;
    [SerializeField] private GameObject movepathTile;
    [SerializeField] private GameObject moveTile;
    [SerializeField] private GameObject attackRangeTile;

    [HideInInspector] public bool canClick;//이동 가능 혹은 상호작용 가능한 타일

    public Vector3Int gridLocation;//타일 위치(월드 위치랑 다름)
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public CharacterBase curStandingCharater;

    private void Start()
    {
        HideTile();
    }

    public void ResetTile()//기본 타일을 제외한 다른 타일 숨기기
    {
        if (canClick)
        {
            defaultTile.SetActive(true);
            movepathTile.SetActive(false);
            moveTile.SetActive(false);
            attackRangeTile.SetActive(false);
        }
    }

    public void ShowTile()//기본 타일 보기(하얀색)
    {
        if (canClick)
        {
            defaultTile.SetActive(true);
        }
    }

    public void ShowAsMove()//이동 타일 보기(파란색)
    {
        if (canClick)
        {
            moveTile.SetActive(true);
            defaultTile.SetActive(!moveTile.activeSelf);
        }
    }

    public void ShowAsScale()//범위 타일 보기(노란색)
    {
        if (canClick)
        {
            movepathTile.SetActive(true);
            defaultTile.SetActive(!movepathTile.activeSelf);
        }
    }

    public void ShowAsAttack()//공격 타일 보기(빨간색)
    {
        if (canClick)
        {
            attackRangeTile.SetActive(true);
            defaultTile?.SetActive(!attackRangeTile.activeSelf);
        }
    }

    public void HideTile()//타일 전체 숨기기
    {
        defaultTile.SetActive(false);
        movepathTile.SetActive(false);
        moveTile.SetActive(false);
        attackRangeTile.SetActive(false);
    }
}
