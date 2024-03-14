using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TraitData", fileName = "Trait_")]
public class TraitSO : ScriptableObject
{
    [Header("Develope")]
    public string trait_Id;//캐릭터 식별자

    [Header("Trait_description")]
    public string traitName;//특성 이름
    public string traitDescription;//특성 설명

    [Header("Image")]
    public Sprite traitSprite;//특성 스프라이트(=썸네일or아이콘)

    [Header("Trait_Status")]
    public Dictionary<int, int> traitStatus = new Dictionary<int, int>();

    //[Header("Trait_function")]


}
