using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/GiftData", fileName = "Gift_")]
public class GiftSO : ItemData
{
    public int affectionValue; //������Ű�� ȣ���� ��ġ
    public int[] favoriteCharacterId; //�� ������ �����ϴ� ĳ���͵��� ID
    public GiftSO()
    {
        itemType = Constants.ItemType.Gift;
    }
}


