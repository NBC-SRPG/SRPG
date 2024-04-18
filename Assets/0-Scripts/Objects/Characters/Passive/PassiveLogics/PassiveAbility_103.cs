using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_103 : PassiveLogic
{
    //ID 1003. 제네 Gene
    //패시브 스킬

    //protected CharacterBase character;

    //public Dictionary<string, int> coefficient;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        base.OnAttackSuccess(enemy, damage);

        int randomValue = UnityEngine.Random.Range(0, 2); // 0 또는 1 반환
        if (randomValue == 0) // 1/2 확률로 실행
        {
            enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2); //2턴 동안 화상 상태이상 적용
        }
    }


    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        base.OnSkillAttackSuccess(target, damage);

        int randomValue = UnityEngine.Random.Range(0, 2); // 0 또는 1 반환
        if (randomValue == 0) // 1/2 확률로 실행
        {
            target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2); //2턴 동안 화상 상태이상 적용
        }
    }
}

