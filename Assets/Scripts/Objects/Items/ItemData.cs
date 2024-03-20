using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    [Header("Item_description")]
    public string itemName;//아이템 이름
    public string itemDescription;//아이템 설명

    [Header("Image")]
    public Sprite itemSprite;//아이템 스프라이트(=썸네일or아이콘)

    [Header("Develope")]
    public int item_Id;//아이템 식별자
    public ItemType itemType; //아이템 분류
    public ItemRank itemRank; //아이템 등급
    public int maxReservesNumbers; //아이템 보유 갯수 최대치
    //public int reservesNumbers; //아이템을 현재 보유하고 있는 갯수
}

