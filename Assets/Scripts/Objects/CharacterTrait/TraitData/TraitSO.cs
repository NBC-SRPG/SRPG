using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "TraitData", fileName = "Trait_")]
public class TraitSO : ScriptableObject
{
    [Header("Develope")]
    public string trait_Id;//캐릭터 식별자
    public TraitType traitType; //특성 분류

    [Header("Trait_description")]
    public string traitName;//특성 이름
    public string traitDescription;//특성 설명
    
    [Header("Image")]
    public Sprite traitSprite;//특성 스프라이트(=썸네일or아이콘)

    [Header("Trait_Status_basic")] //increase = 깡스탯 증가량(합연산). multiply = 배율(%) 증가량(곱연산)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public int multiplyHealth;
    public int multiplyAtk;
    public int multiplyDef;

    [Header("Trait_Status_addition")]
    public float increasecCtr; //치명타 확률 +
    public float increasecCtd; //치명타 피해 +
    public float increaseInflictDamage; //주는 최종 피해 증가 +
    public float reducedTakenDamage; //받는 최종 피해 감소 -


    [Header("Trait_special")]
    public string speciaID;//특성의 별도 고유 효과
}
