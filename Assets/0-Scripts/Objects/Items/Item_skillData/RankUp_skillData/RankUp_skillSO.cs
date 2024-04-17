using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/skill/RankUp_skillData", fileName = "ItemSO_")]
public class RankUp_skillSO : ItemSO
{
    public Faction belonging; //���� ����

    public RankUp_skillSO()
    {
        itemType = Constants.ItemType.RankUp_skill;
    }

}

