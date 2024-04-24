using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_722: PassiveLogic
{
    //"연계 공격"
    //적을 행동 불능 상태로 만들  때마다, 공격력이  10%  증가한다.
    //이 로직 자체로는 아무 의미가 없고, 패시브 스크립트에서 로직을 처리함.
    //시스의 2-2 특성
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

}

