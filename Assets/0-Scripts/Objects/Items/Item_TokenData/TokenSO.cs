using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/TokenData", fileName = "Token_")]
public class TokenSO : ItemSO
{
    public int eventId; //�̺�Ʈ ID
    public TokenSO() 
    {
        itemType = Constants.ItemType.Token;
    }
}
