using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_521: PassiveLogic
{
    //"무장 해제"
    //[부식]상태의 적은 중첩당 방어력이 3% 감소한다
    //이 특성 로직만으로는 아무 효과가 없고, 실제 효과는 부식 디버프 로직에서 조건을 체크함.
    //레인의 2-1 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }
}

