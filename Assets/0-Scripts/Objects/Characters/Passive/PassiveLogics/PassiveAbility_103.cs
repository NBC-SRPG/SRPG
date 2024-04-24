using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_103 : PassiveLogic
{
    //ID 003. 제네 Gene
    //패시브 스킬
    //공격한 적에게 확률에 따라 2턴간 "화상" 상태이상을 부여한다.
    //3티어 특성 "불타버려!!"를 적용 중일 경우, 확정으로 화상을 부여한다.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        if (character.character.abilityT3 != null && character.character.abilityT3.id == coefficient["abilityId_T3"]) //현재 331번 특성 "불타버려!!"가 적용 중일 경우, 확정으로 화상을 부여한다.
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, coefficient["debufDuration"], character); //2턴 동안 화상 상태이상 적용
        } 
        else
        {
            int randomValue = UnityEngine.Random.Range(coefficient["probabilityConstant1"], coefficient["probabilityConstant2"]); // 0 또는 1 반환
            if (randomValue == coefficient["probabilityConstant1"]) // 1/2 확률로 실행
            {
                enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, coefficient["debufDuration"], character); //2턴 동안 화상 상태이상 적용
            }
        }
    }


    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        if (character.character.abilityT3 != null && character.character.abilityT3.id == coefficient["abilityId_T3"]) //현재 331번 특성 "불타버려!!"가 적용 중일 경우, 확정으로 화상을 부여한다.
        {
            target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, coefficient["debufDuration"], character); //2턴 동안 화상 상태이상 적용
        }
        else
        {
            int randomValue = UnityEngine.Random.Range(coefficient["probabilityConstant1"], coefficient["probabilityConstant2"]); // 0 또는 1 반환
            if (randomValue == coefficient["probabilityConstant1"]) // 1/2 확률로 실행
            {
                target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, coefficient["debufDuration"], character); //2턴 동안 화상 상태이상 적용
            }
        }
    }

}

