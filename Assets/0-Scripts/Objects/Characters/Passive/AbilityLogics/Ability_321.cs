using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_321: PassiveLogic
{
    //"꺼지지 않는 마음"
    //턴의 종료에, 잃은 체력에 비례한 체력을 회복한다.
    //제네의 2-1 특성

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {
        int LostHealth = ((character.health.MaxHealth - character.health.CurHealth)); //잃은 체력을 구한다
        character.health.HealHealthByInt((int)(LostHealth * (coefficient["healRate"]/100))); //잃은 체력의 10% 회복
    }
}

