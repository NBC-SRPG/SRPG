using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    [Header("Item_description")]
    [SerializeField]
    protected string itemName;//아이템 이름
    [SerializeField]
    protected string itemDescription;//아이템 설명

    [Header("Image")]
    [SerializeField]
    protected Sprite itemSprite;//아이템 스프라이트(=썸네일or아이콘)

    [Header("Develope")]
    [SerializeField]
    protected int item_Id;//아이템 식별자
    [SerializeField]
    protected int maxReservesNumbers; //아이템 보유 갯수 최대치
    [SerializeField]
    protected ItemType itemType; //아이템 분류
    [SerializeField]
    protected ItemRank itemRank; //아이템 등급
    [SerializeField]
    protected int price; //아이템 가격 (상점 가격)
    public int reservesNumbers; //아이템을 현재 보유하고 있는 갯수
}

