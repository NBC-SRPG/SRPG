using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_922: PassiveLogic
{
    //"리사이클"
    //적을 처치했을 경우 비용을 26 회복한다.
    //에오스의 2-2 특성

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }

    public override void OnKillEnemy(CharacterBase enemy, Constants.ElementType characterAttribute = Constants.ElementType.None)// 적 처치 시
    {
        Debug.Log("kill");
        character.player.GainMana(coefficient["costRecovery"]);
    }

}

