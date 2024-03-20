using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/equip/RankUp_equipData/RankUp_equip_armor", fileName = "RankUp_equip_armor_")]
public class RankUp_equip_armorSO : ItemData
{
    public ArmorType armorType;

    public RankUp_equip_armorSO()
    {
        itemType = Constants.ItemType.RankUp_equip_armor;
    }
}
