using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "Item_")]
public class ItemSO : ScriptableObject
{
    [Header("Item_description")]
    public string itemName;//아이템 이름
    public string itemDescription;//아이템 설명

    [Header("Develope")]
    public int id;//아이템 식별자
    public ItemType itemType; //아이템 분류

}