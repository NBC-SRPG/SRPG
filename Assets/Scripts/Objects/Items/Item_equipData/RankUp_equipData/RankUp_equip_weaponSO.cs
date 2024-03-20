using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/equip/RankUp_equipData/RankUp_equip_weapon", fileName = "RankUp_equip_weapon_")]
public class RankUp_equip_weaponSO : ItemData
{
    public WeaponType weaponType; 

    public RankUp_equip_weaponSO()
    {
        itemType = Constants.ItemType.RankUp_equip_weapon;
    }
}

