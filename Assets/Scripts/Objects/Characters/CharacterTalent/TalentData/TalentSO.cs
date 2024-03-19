using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "TalentData", fileName = "Talent_")]
public class TalentSO : ScriptableObject
{
    [Header("Develope")]
    public string talent_Id;//Ư�� �ĺ���
    public TalentType talentType; //Ư�� �з�

    [Header("Talent_description")]
    public string talentName;//Ư�� �̸�
    public string talentDescription;//Ư�� ����
    
    [Header("Image")]
    public Sprite talentSprite;//Ư�� ��������Ʈ(=�����or������)

    [Header("Talent_Status_basic")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth; //�������� ����� �� 1�� ���ϰ� �����ϹǷ�, �� Ư������ 10%�� ������Ű�� �ʹٸ� 0.1�� �����ϴ� ������� �����ϸ� �˴ϴ�.
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Talent_Status_addition")]
    public float increasecCtr; //ġ��Ÿ Ȯ�� +
    public float increasecCtd; //ġ��Ÿ ���� +
    public float increaseInflictDamage; //�ִ� ���� ���� ���� +
    public float reducedTakenDamage; //�޴� ���� ���� ���� -


    [Header("Talent_special")]
    public string speciaID;//Ư���� ���� ���� ȿ��. ���η��� �̱���.
                           //Todo: Ư���� ���� ���� �ܿ� Ư�� ����� �����ϰ� ������ �� �ֵ��� ���� ���� / ����
}
