using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/MemorialData", fileName = "ItemSO_")]
public class MemorialSO : ItemSO
{
    public MemorialSO()
        {
            itemType = Constants.ItemType.Memorial;
        }
}
