using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    [SerializeField] private GameObject defaultTile;
    [SerializeField] private GameObject movepathTile;
    [SerializeField] private GameObject moveTile;
    [SerializeField] private GameObject attackRangeTile;

    [HideInInspector] public bool canClick;//�̵� ���� Ȥ�� ��ȣ�ۿ� ������ Ÿ��

    public Vector3Int gridLocation;//Ÿ�� ��ġ(���� ��ġ�� �ٸ�)
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

    public void ShowTile()//�⺻ Ÿ�� ����(�Ͼ��)
    {
        if (canClick)
        {
            defaultTile.SetActive(true);
        }
    }

    public void ShowAsMove()//�̵� Ÿ�� ����(�����)
    {
        if (canClick)
        {
            moveTile.SetActive(true);
        }
    }

    public void ShowAsScale()//���� Ÿ�� ����(�����)
    {
        if (canClick)
        {
            movepathTile.SetActive(true);
        }
    }

    public void ShowAsAttack()//���� Ÿ�� ����(������)
    {
        if (canClick)
        {
            attackRangeTile.SetActive(true);
        }
    }

    public void HideTile()//Ÿ�� ��ü �����
    {
        defaultTile.SetActive(false);
        movepathTile.SetActive(false);
        moveTile.SetActive(false);
        attackRangeTile.SetActive(false);
    }
}
