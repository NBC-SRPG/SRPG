using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_202 : PassiveLogic
{
    //레인저

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        int randomValue = UnityEngine.Random.Range(coefficient["probabilityConstant1"], coefficient["probabilityConstant2"]); // 0 또는 1 반환
        if (randomValue == coefficient["probabilityConstant1"]) // 1/2 확률로 실행
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.DefReduce, coefficient["debufDuration"], character, coefficient["reduceDefRate"]);
        }
    }


    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        int randomValue = UnityEngine.Random.Range(coefficient["probabilityConstant1"], coefficient["probabilityConstant2"]); // 0 또는 1 반환
        if (randomValue == coefficient["probabilityConstant1"]) // 1/2 확률로 실행
        {
            target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.DefReduce, coefficient["debufDuration"], character, coefficient["reduceDefRate"]);
        }
    }
}

