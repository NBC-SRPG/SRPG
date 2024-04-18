using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_321: PassiveLogic
{

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {
        character.health.HealHealthByInt(150);
        //1. 캐릭터 잃은 HP 값을 어떻게 불러오는지
    }
}

