using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_632: PassiveLogic
{
    //"지원 사격"
    //[표적]의 지속시간이 2턴으로 증가한다.
    //이 특성 로직 자체로는 아무 효과도 없고, 피유의 패시브 스크립트에서 해당 조건을 처리함
    //피유의 3-2 특성.


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }
}

