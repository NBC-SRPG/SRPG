using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_522: PassiveLogic
{
    //"휘몰아치는 비"
    //레인의 레인의 일반  공격이 부식을 추가로 2회 중첩시킨다.
    //레인의 2-2 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnAttackSuccess(CharacterBase enemy, BattleKeyWords.Damage damage)// 공격 적중 시
    {
        enemy.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, coefficient["debufPower"], coefficient["debufStack"]); //2턴 동안 2스택의 부식 디버프 적용
    }
}

