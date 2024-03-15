using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ClassData", fileName = "Class_")]
public class ClassSO : ScriptableObject
{
    [Header("Develope")]
    public string class_Id;//Ŭ���� �ĺ���
    public BaseClass bassClass; //�Ѹ� Ŭ����

    [Header("Trait_description")]
    public string className;//Ŭ���� �̸�
    public string classDescription;//Ŭ���� ����

    [Header("Image")]
    public Sprite traitSprite;//Ŭ���� ��������Ʈ(=�����or������)

    [Header("Class_Status_basic")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth = 1;
    public float multiplyAtk = 1;
    public float multiplyDef = 1;

    [Header("Trait_Status_addition")]
    public float increasecCtr; //ġ��Ÿ Ȯ�� +
    public float increasecCtd; //ġ��Ÿ ���� +
    public float increaseInflictDamage; //�ִ� ���� ���� ���� +
    public float reducedTakenDamage; //�޴� ���� ���� ���� -

    [Header("Class_unique")]
    public string uniqueID;//Ŭ������ ���� ���� ȿ��
}
