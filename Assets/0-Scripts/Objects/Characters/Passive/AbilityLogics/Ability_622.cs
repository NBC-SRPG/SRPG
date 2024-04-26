using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_622: PassiveLogic
{
    //"지원 사격"
    //아군이 표적을 공격시, 대상이 피유의 사거리 내에 있다면, 피유가 50%의 공격력으로 1회 공격한다.
    //이 특성 로직 자체로는 아무 효과도 없고, [표적] 디버프 스크립트에서 해당 조건을 처리함
    //피유의 2-1 특성.


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }
}

