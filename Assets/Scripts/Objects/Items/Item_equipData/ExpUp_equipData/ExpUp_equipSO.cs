using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData/equip/ExpUp_equipData", fileName = "ExpUp_equip_")]
public class ExpUp_equipSO : ItemData
{
    public int expValue;
    public ExpUp_equipSO() 
    {
        itemType = Constants.ItemType.ExpUp_equip;
    }
}

