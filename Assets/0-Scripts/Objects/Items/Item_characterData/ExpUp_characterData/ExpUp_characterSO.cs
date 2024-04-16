using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData/character/ExpUp_characterData", fileName = "ExpUp_character_")]
public class ExpUp_characterSO : ItemSO
{
    public int expValue;

    public ExpUp_characterSO()
    {
        itemType = ItemType.ExpUp_character;
    }
}