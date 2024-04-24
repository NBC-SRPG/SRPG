using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability_631: PassiveLogic
{
    //"민첩한 발걸음"
    //자신의 Mov가  + 1 증가한다.
    //피유의 3-1 특성.

    BonusStat stat_631 = new BonusStat(); // 보너스 스탯

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_631.ExtraMov = coefficient["extraMovIncreasse"];
        character.tempBonusStat.AddBonusStat(stat_631);
    }
}

