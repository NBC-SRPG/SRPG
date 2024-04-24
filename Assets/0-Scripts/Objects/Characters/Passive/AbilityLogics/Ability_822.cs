using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static BattleKeyWords;

public class Ability_822: PassiveLogic
{
    //"방패 재생"
    //턴, 종료시,  최대체력의  10%에  해당하는 [자수정 방패]를 얻는다. 1턴 동안 유지된다.
    //아메의 2-1 특성
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnTurnEnd()// 턴이 끝날 때
    {
        character.curCharacterBufList.AddBuf(BufKeyword.AmethystShield, coefficient["shieldDuration"], character, coefficient["shieldPower"], (int)(character.health.TotalHealth * (float)coefficient["hpRatio"]/ coefficient["denominator"]));
    }
}

