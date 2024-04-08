using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/MemorialData", fileName = "Memorial_")]
public class MemorialSO : ItemData
{
    public MemorialSO()
        {
            itemType = Constants.ItemType.Memorial;
        }
}
