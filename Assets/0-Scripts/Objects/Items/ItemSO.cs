using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "ItemSO_")]
public class ItemSO : ScriptableObject
{
    [Header("Item_description")]
    public string itemName;//아이템 이름
    public string itemDescription;//아이템 설명
    public Sprite icon;

    [Header("Develope")]
    public int id;//아이템 식별자
    public ItemType itemType; //아이템 분류

}