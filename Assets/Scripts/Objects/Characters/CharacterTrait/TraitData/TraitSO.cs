using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "TraitData", fileName = "Trait_")]
public class TraitSO : ScriptableObject
{
    [Header("Develope")]
    public string trait_Id;//ĳ���� �ĺ���
    public TraitType traitType; //Ư�� �з�

    [Header("Trait_description")]
    public string traitName;//Ư�� �̸�
    public string traitDescription;//Ư�� ����
    
    [Header("Image")]
    public Sprite traitSprite;//Ư�� ��������Ʈ(=�����or������)

    [Header("Trait_Status_basic")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth; //�������� ����� �� 1�� ���ϰ� �����ϹǷ�, �� Ư������ 10%�� ������Ű�� �ʹٸ� 0.1�� �����ϴ� ������� �����ϸ� �˴ϴ�.
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Trait_Status_addition")]
    public float increasecCtr; //ġ��Ÿ Ȯ�� +
    public float increasecCtd; //ġ��Ÿ ���� +
    public float increaseInflictDamage; //�ִ� ���� ���� ���� +
    public float reducedTakenDamage; //�޴� ���� ���� ���� -


    [Header("Trait_special")]
    public string speciaID;//Ư���� ���� ���� ȿ��. ���η��� �̱���.
                           //Todo: Ư���� ���� ���� �ܿ� Ư�� ����� �����ϰ� ������ �� �ֵ��� ���� ���� / ����
}
