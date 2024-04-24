using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_421: PassiveLogic
{
    //"마음을 담은 기도"
    //자신의 치유 스킬에 영향을 받은 적이 받는 피해가 10% 증가하고,
    //아군이 받는 피해가 10% 감소한다. 1턴간 지속된다. ※보너스 스탯이 아니라 디버프/버프를 거는 판정임.
    //포르테의 2-1 특성.




    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 사용 시
    {

    }


    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        if (character.character.abilityT3.id == coefficient["abilityIdT3"]) //현재 432번 특성 "신의 축복"이 적용 중일 경우
        {
            if (character.CheckEnemy(target))
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.ReceivedDgmIncrease, coefficient["debufDuration_2"], character, coefficient["debufCoefficients_2"]);//적일 경우 2턴 간 받는 피해 15% 증가
            }
            else
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.ReceivedDgmReduce, coefficient["bufDuration_2"], character, coefficient["bufCoefficients_2"]); //아군일 경우 2턴 간 받는 피해 15% 감소
            }
        }
        else
        {
            if (character.CheckEnemy(target))
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.ReceivedDgmIncrease, coefficient["debufDuration"], character, coefficient["debufCoefficients"]);//적일 경우 받는 피해 10% 증가
            }
            else
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.ReceivedDgmReduce, coefficient["bufDuration"], character, coefficient["bufCoefficients"]); //아군일 경우 받는 피해 10% 감소
            }
        }
    }

    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {

    }


    public override void OnTakeHeal(ref int damage, CharacterBase enemy = null,
        BattleKeyWords.AttackDamageType damageType = BattleKeyWords.AttackDamageType.None,
        Constants.ElementType characterAttribute = Constants.ElementType.None)// 힐을 받을 때
    {

    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {

    }

}

