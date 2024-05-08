using System.Collections.Generic;
using UnityEngine;
using static Constants;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ItemData", fileName = "ItemSO_")]
public class ItemSO : SerializedScriptableObject
{
    [Header("Item_description")]
    public string itemName;//아이템 이름
    public string itemDescription;//아이템 설명
    public Sprite icon;

    [Header("Develope")]
    public int id;//아이템 식별자
    public ItemType itemType; //아이템 분류

    [Header("Additional")]
    [Tooltip("딕셔너리에 타입별 값을 추가해 사용")]
    public Dictionary<string, int> values;

}