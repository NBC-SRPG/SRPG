using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Trait_Status")]
    public Dictionary<int, int> traitStatus = new Dictionary<int, int>();

    //[Header("Trait_function")]


}
