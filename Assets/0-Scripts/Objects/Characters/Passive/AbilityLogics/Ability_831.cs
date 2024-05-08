using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_831: PassiveLogic
{
    //"면역의 방패"
    //자신이 부여한 [자수정 방패] 효과를 가진 아군은 모든 상태 이상 및 디버프 효과에 면역이 된다. 
    //이 로직 자체로는 아무 효과가 없고, 버프 [자수정 방패] 스크립트에서 해당 로직을 처리한다.
    //아메의 3-1 특성


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }
}

