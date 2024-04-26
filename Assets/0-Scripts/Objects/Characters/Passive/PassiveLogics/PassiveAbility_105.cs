using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_105 : PassiveLogic
{
    //ID 005. 레인 Rain
    //패시브 스킬
    //레인의 일반 공격은 적에게 2턴동안 [부식] 상태이상을 부여한다.  ( 부식 :  매 턴이 시작 될 때마다 중첩당 최대체력의 2% 데미지를 입는다. 최대 중첩 수 : 99)
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, coefficient["debufPower"], coefficient["debufStack"]); //2턴 동안 1스택의 부식 디버프 적용
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, coefficient["debufPower"], coefficient["debufStack_Skill"]); //2턴 동안 3스택의 부식 디버프 적용.
    }
}

