using System.Collections.Generic;
using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "EquipData", fileName = "Equip_")]
public class EquipSO : ScriptableObject
{
    [Header("Equip_description")]
    public string equipName;//��� �̸�
    public string equipDescription;//��� ����
    

    [Header("Equip_Status_basic")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth; //�������� ����� �� 1�� ���ϰ� �����ϹǷ�, �� Ư������ 10%�� ������Ű�� �ʹٸ� 0.1�� �����ϴ� ������� �����ϸ� �˴ϴ�.
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Equip_Status_addition")]
    public float increasecCtr; //ġ��Ÿ Ȯ�� +
    public float increasecCtd; //ġ��Ÿ ���� +
    public float increaseInflictDamage; //�ִ� ���� ���� ���� +
    public float reducedTakenDamage; //�޴� ���� ���� ���� -


    [Header("Equip_special")]
    public int speciaID;//����� ���� ���� ȿ��. ���η��� �̱���.

    [Header("Develope")]
    public int equip_Id;//��� �ĺ���
    public EquipType equipType; //��� Ÿ��
    public ItemRank ItemRank;
    public Dictionary<int, int> rankUpMaterials; //�����ϴµ� �ʿ��� ���� �������� ID�� / �ʿ��� ���� ��ųʸ�
}
