using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_432: PassiveLogic
{
    //"신의 축복"
    //자신이 적과 아군에게 주는 모든 디버프 / 버프 효과의 지속시간이 1턴 증가하고, 효과 수치가 1.5배로 된다.
    //"버프를 줄 때" 조건이 없어서, 이 특성 자체로는 아무런 효과가 없고 이 특성이 적용 중일 시의 조건을 포르테의 버프를 주는 다른 특성 로직에 추가했습니다.
    //포르테의 3-2 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

}

