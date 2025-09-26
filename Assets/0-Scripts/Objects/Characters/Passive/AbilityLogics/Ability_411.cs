using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_411: PassiveLogic
{
    //"축성의 노래"
    //자신이 치유한 유닛 1명 당 코스트를 5 획득한다.
    //포르테의 1 특성.


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnSkillHealSuccess(CharacterBase target, BattleKeyWords.Damage heal)// 스킬로 체력 회복 시
    {
        character.player.GainMana(coefficient["gainMana"]);
    }
}

