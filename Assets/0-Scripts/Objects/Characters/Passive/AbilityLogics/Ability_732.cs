using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_732: PassiveLogic
{
    //"축전"
    //아군이 이번 턴에  Ex스킬을 사용하기 위해 소모한 분노의 50%만큼 ‘내리치는 벼락’의 비용이 감소한다.
    //시스의 3-2 특성

    int prevManaCost;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

        prevManaCost = character.curCharacterSkill.skillData.cost;
    }

    public override void OnRoundStart()// 
    {

    }

    public override void OnTurnStart()// 턴 시작 시 발동
    {
        character.skillCost = character.curCharacterSkill.skillData.cost;
    }

    public override void OnUpdate()// 실시간 판정
    {
        //if(character.player.manaCost != prevManaCost)
        //{
        //    if (character.player.manaCost < prevManaCost)
        //    {
        //        character.skillCost -= ((int)(prevManaCost - character.player.manaCost) / coefficient["denominator"]);
        //        prevManaCost = character.player.manaCost;
        //    }
        //    else if (character.player.manaCost > prevManaCost)
        //    {
        //        prevManaCost = character.player.manaCost;
        //    }
        //}

        if(character.player.UsedManaThisTurn != 0 && prevManaCost == character.skillCost)
        {
            character.skillCost -= (int)(character.player.UsedManaThisTurn / coefficient["denominator"]);
        }
    }
}

