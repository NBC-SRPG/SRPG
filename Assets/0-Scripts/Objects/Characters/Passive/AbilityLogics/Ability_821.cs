using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_821: PassiveLogic
{
    //"마력 운용"
    //자신의 Ex스킬의 비용이 10 감소한다.
    //아메의 2-1 특성
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

    }

    public override void OnStageStart()
    {
        character.skillCost = character.curCharacterSkill.skillData.cost;
        character.skillCost -= coefficient["reduceCost"];
    }
}

