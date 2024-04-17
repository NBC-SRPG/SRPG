using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/ConsumableData", fileName = "ItemSO_")]
public class ConsumableSO : ItemSO
{
    public Rewards openRewards; //���� ����
    public int openRewardCount; //���� ����

    public ConsumableSO()
    {
        itemType = Constants.ItemType.Consumable;
    }
}



