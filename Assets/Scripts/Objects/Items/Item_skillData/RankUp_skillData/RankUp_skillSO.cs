using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/skill/RankUp_skillData", fileName = "RankUp_skill_")]
public class RankUp_skillSO : ItemData
{
    public Belonging belonging; //진영 정보

    public RankUp_skillSO()
    {
        itemType = Constants.ItemType.RankUp_skill;
    }

}

