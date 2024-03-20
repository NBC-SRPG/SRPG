using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/PieceData", fileName = "Piece_")]
public class PieceSO : ItemData
{
    public int characterId; //�� ������ ���� ĳ���� ID
    public PieceSO()
    {
        itemType = Constants.ItemType.Piece;
    }
}
