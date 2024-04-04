using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/ConsumableData", fileName = "Consumable_")]
public class ConsumableSO : ItemData
{
    public Rewards openRewards; //보상 종류
    public int openRewardCount; //보상 갯수

    //Todo: 소비 아이템 사용 시 해당 재화가 플레이어에게 지급되도록 기능 구현

    public ConsumableSO()
    {
        itemType = Constants.ItemType.Consumable;
    }
}



