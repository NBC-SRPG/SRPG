using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/GiftData", fileName = "ItemSO_")]
public class GiftSO : ItemSO
{
    public int affectionValue; //증가시키는 호감도 수치
    public int[] favoriteCharacterId; //이 선물을 좋아하는 캐릭터들의 ID
    public GiftSO()
    {
        itemType = Constants.ItemType.Gift;
    }
}