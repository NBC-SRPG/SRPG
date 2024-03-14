using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TraitType
{
    Stat, //�ܼ��� ���� (ü��/����/���ݷ�, MOV, ġȮ, ġ��, ����, ���ǰ�) ��ġ�� ��½����ݴϴ�.
    Special //���� �ܿ� �ΰ����� Ư�� ��Ŀ������ �߰��ϰų� ���� ��ŵ�ϴ�. 
}

[CreateAssetMenu(menuName = "TraitData", fileName = "Trait_")]
public class TraitSO : ScriptableObject
{
    [Header("Develope")]
    public string trait_Id;//ĳ���� �ĺ���

    [Header("Trait_description")]
    public string traitName;//Ư�� �̸�
    public string traitDescription;//Ư�� ����

    [Header("Image")]
    public Sprite traitSprite;//Ư�� ��������Ʈ(=�����or������)

    [Header("Trait_Status")] //increase = ������ ������(�տ���). multiply = ����(%) ������(������)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public int multiplyHealth;
    public int multiplyAtk;
    public int multiplyDef;
    public int multiplyMov;

    [Header("Trait_special")]
    public string speciaID;//Ư��

}
