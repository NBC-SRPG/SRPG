using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "ArmorData", fileName = "Armor_")]
public class ArmorSO : EquipSO
{
    [Header("Develope")]
    public string aromor_Id;//Ư�� �ĺ���
    public TraitType armorType; //Ư�� �з�

    /*
    [Header("Equip_description")]
    public string equipName;//Ư�� �̸�
    public string equipDescription;//Ư�� ����
    */
    
    [Header("Image")]
    public Sprite equipSprite;//Ư�� ��������Ʈ(=�����or������)

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
    public string speciaID;//Ư���� ���� ���� ȿ��. ���η��� �̱���.
                           //Todo: Ư���� ���� ���� �ܿ� Ư�� ����� �����ϰ� ������ �� �ֵ��� ���� ���� / ����
}
