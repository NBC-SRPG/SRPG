using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ClassData", fileName = "Class_")]
public class ClassSO : ScriptableObject
{
    [Header("Develope")]
    public int class_Id;//Ŭ���� �ĺ���
    public BaseClass bassClass; //�Ѹ� Ŭ����

    [Header("Class_description")]
    public string className;//Ŭ���� �̸�
    public string classDescription;//Ŭ���� ����

    [Header("Class_Status_basic")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth;
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Class_Status_addition")]
    public float increasecCtr; //ġ��Ÿ Ȯ�� +
    public float increasecCtd; //ġ��Ÿ ���� +
    public float increaseInflictDamage; //�ִ� ���� ���� ���� +
    public float reducedTakenDamage; //�޴� ���� ���� ���� -

    [Header("Class_unique")]
    public int uniqueID;//Ŭ������ ���� ���� ȿ��. ���η��� �̱���.
                           //Todo: Ư���� ���� ���� �ܿ� Ư�� ����� �����ϰ� ������ �� �ֵ��� ���� ���� / ����
}
