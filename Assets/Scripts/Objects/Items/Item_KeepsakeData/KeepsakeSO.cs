using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/KeepsakeData", fileName = "Keepsake_")]
public class KeepsakeSO : ItemData
{
    public KeepsakeSO()
        {
            itemType = Constants.ItemType.Keepsake;
        }
}
