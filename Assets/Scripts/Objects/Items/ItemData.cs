using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    [Header("Item_description")]
    public string itemName;//������ �̸�
    public string itemDescription;//������ ����

    [Header("Image")]
    public Sprite itemSprite;//������ ��������Ʈ(=�����or������)

    [Header("Develope")]
    public int item_Id;//������ �ĺ���
    public ItemType itemType; //������ �з�
    public ItemRank itemRank; //������ ���
    public int maxReservesNumbers; //������ ���� ���� �ִ�ġ
    //public int reservesNumbers; //�������� ���� �����ϰ� �ִ� ����
}

