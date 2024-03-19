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
    public string item_Id;//������ �ĺ���
    public int reservesNumbers; //�������� ���� �����ϰ� �ִ� ����
    public int maxReservesNumbers; //������ ���� ���� �ִ�ġ
    public ItemType itemType; //������ �з�
    public ItemRank itemRank; //������ ���
    public int price; //������ ���� (���� ����)
}

