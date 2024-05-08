using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_531: PassiveLogic
{
    //"폭풍쇄도"
    //비바라기가 부식을 3회
    //추가로 중첩시킨다. 
    //레인의 3-1 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnSkillAttackSuccess(CharacterBase target, BattleKeyWords.Damage damage)// 스킬 적중 시
    {
        target.curCharacterBufList.AddBuf(BattleKeyWords.BufKeyword.Corrosion, coefficient["debufDuration"], character, coefficient["debufPower"], coefficient["debufStack"]); //2턴 동안 3스택의 부식 디버프 적용
    }
}

