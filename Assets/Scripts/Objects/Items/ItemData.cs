using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "ItemData", fileName = "Item_")]
public class ItemData : ScriptableObject
{
    [Header("Item_description")]
    [SerializeField]
    protected string itemName;//������ �̸�
    [SerializeField]
    protected string itemDescription;//������ ����

    [Header("Image")]
    [SerializeField]
    protected Sprite itemSprite;//������ ��������Ʈ(=�����or������)

    [Header("Develope")]
    [SerializeField]
    protected int item_Id;//������ �ĺ���
    [SerializeField]
    protected int maxReservesNumbers; //������ ���� ���� �ִ�ġ
    [SerializeField]
    protected ItemType itemType; //������ �з�
    [SerializeField]
    protected ItemRank itemRank; //������ ���
    [SerializeField]
    protected int price; //������ ���� (���� ����)
    public int reservesNumbers; //�������� ���� �����ϰ� �ִ� ����
}

