using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/PieceData", fileName = "ItemSO_")]
public class PieceSO : ItemSO
{
    public int characterId; //�� ������ ���� ĳ���� ID
    public PieceSO()
    {
        itemType = Constants.ItemType.Piece;
    }
}
