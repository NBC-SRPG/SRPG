using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ClassData", fileName = "Class_")]
public class ClassSO : ScriptableObject
{
    [Header("Develope")]
    public string class_Id;//클래스 식별자
    public BaseClass bassClass; //뿌리 클래스

    [Header("Trait_description")]
    public string className;//클래스 이름
    public string classDescription;//클래스 설명

    [Header("Image")]
    public Sprite classSprite;//클래스 스프라이트(=썸네일or아이콘)

    [Header("Class_Status_basic")] //increase = 깡스탯 증가량(합연산). multiply = 배율(%) 증가량(곱연산)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth;
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Trait_Status_addition")]
    public float increasecCtr; //치명타 확률 +
    public float increasecCtd; //치명타 피해 +
    public float increaseInflictDamage; //주는 최종 피해 증가 +
    public float reducedTakenDamage; //받는 최종 피해 감소 -

    [Header("Class_unique")]
    public string uniqueID;//클래스의 별도 고유 효과. 세부로직 미구현.
                           //Todo: 특성이 스탯 증가 외에 특수 기능을 구현하고 적용할 수 있도록 로직 설계 / 구현
}
