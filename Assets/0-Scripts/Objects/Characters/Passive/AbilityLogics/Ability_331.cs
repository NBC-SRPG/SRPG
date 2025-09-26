using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_331: PassiveLogic
{
    //"불타버려!!"
    //이 클래스 스크립트 자체로는 아무 효과도 없고, "태초의 불꽃" (PassiveAbility_103)스크립트에서
    //현재 캐릭터가 3티어 특성으로 331번 어빌리티를 선택 중일 경우 화상 부여 확률을 100%로 변경한다.
    //패시브 스킬 "태초의 불꽃"을 강화하는 제네의 전용 특성.
    //제네의 3-1 특성.

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }
}

