using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/equip/RankUp_equipData", fileName = "ItemSO_")]
public class RankUp_equipSO : ItemSO
{
    public RankUp_equipSO()
    {
        itemType = Constants.ItemType.RankUp_equip;
    }
}

