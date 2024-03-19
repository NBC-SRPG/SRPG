using UnityEngine;
using static Constants;



[CreateAssetMenu(menuName = "TalentData", fileName = "Talent_")]
public class TalentSO : ScriptableObject
{
    [Header("Develope")]
    public string talent_Id;//특성 식별자
    public TalentType talentType; //특성 분류

    [Header("Talent_description")]
    public string talentName;//특성 이름
    public string talentDescription;//특성 설명
    
    [Header("Image")]
    public Sprite talentSprite;//특성 스프라이트(=썸네일or아이콘)

    [Header("Talent_Status_basic")] //increase = 깡스탯 증가량(합연산). multiply = 배율(%) 증가량(곱연산)
    public int increaseHealth;
    public int increaseAtk;
    public int increaseDef;
    public int increasecMov;

    public float multiplyHealth; //곱연산은 계산할 때 1을 더하고 시작하므로, 이 특성으로 10%를 증가시키고 싶다면 0.1로 설정하는 방식으로 설정하면 됩니다.
    public float multiplyAtk;
    public float multiplyDef;

    [Header("Talent_Status_addition")]
    public float increasecCtr; //치명타 확률 +
    public float increasecCtd; //치명타 피해 +
    public float increaseInflictDamage; //주는 최종 피해 증가 +
    public float reducedTakenDamage; //받는 최종 피해 감소 -


    [Header("Talent_special")]
    public string speciaID;//특성의 별도 고유 효과. 세부로직 미구현.
                           //Todo: 특성이 스탯 증가 외에 특수 기능을 구현하고 적용할 수 있도록 로직 설계 / 구현
}
