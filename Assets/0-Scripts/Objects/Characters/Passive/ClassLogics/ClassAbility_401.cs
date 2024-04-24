using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_401 : PassiveLogic
{
    //현자
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.player.GainMana((int)(character.skillCost * (float)(coefficient["costRecoveryRate"]) / coefficient["denominator"]));
    }
}

