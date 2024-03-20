using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/character/RankUp_characterData", fileName = "RankUp_character_")]
public class RankUp_characterSO : ItemData
{
    public Belonging belonging; //진영 정보

    public RankUp_characterSO()
    {
        itemType = Constants.ItemType.RankUp_character;
    }
}


