using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/PieceData", fileName = "Piece_")]
public class PieceSO : ItemData
{
    public int characterId; //이 조각의 주인 캐릭터 ID
    public PieceSO()
    {
        itemType = Constants.ItemType.Piece;
    }
}
