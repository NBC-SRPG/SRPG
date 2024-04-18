using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_103 : PassiveLogic
{
    //No.1 제네 Gene
    //패시브 스킬

    //protected CharacterBase character;

    //public Dictionary<string, int> coefficient;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2); //2턴 동안 화상 상태이상 적용
    }


    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Burn, 2); //2턴 동안 화상 상태이상 적용
    }
}

