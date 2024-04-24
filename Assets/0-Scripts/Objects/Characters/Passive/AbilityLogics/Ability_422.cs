using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_422: PassiveLogic
{
    //"은총"
    //포르테의 치유를 받은 아군은 1턴 동안 치명타 확률이 10% 증가한다.
    //포르테의 2-2 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        if (!character.CheckEnemy(target)) //적이 아닐 경우
        {
            if (character.character.abilityT3.id == coefficient["abilityId_T3"]) //현재 432번 특성 "신의 축복"이 적용 중일 경우
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.CrtRateIncrease, coefficient["bufDuration_2"], character, coefficient["bufCoefficients_2"]); //아군일 경우 2턴 간 치명타 확률 15%증가
            }
            else 
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.CrtRateIncrease, coefficient["bufDuration"], character, coefficient["bufCoefficients"]); //아군일 경우 1턴 간 치명타 확률 10%증가
            }
        }
    }

}

