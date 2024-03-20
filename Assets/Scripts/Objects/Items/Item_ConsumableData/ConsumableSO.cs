using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/ConsumableData", fileName = "Consumable_")]
public class ConsumableSO : ItemData
{
    public Rewards openRewards; //���� ����
    public int openRewardCount; //���� ����

    //Todo: �Һ� ������ ��� �� �ش� ��ȭ�� �÷��̾�� ���޵ǵ��� ��� ����

    public ConsumableSO()
    {
        itemType = Constants.ItemType.Consumable;
    }
}



