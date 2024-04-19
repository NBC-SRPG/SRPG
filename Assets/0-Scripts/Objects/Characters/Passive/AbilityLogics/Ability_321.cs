using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_321: PassiveLogic
{
    //"꺼지지 않는 마음"
    //턴의 종료에, 잃은 체력에 비례한 체력을 회복한다.
    //제네의 2-1 특성


    List<CharacterBase> characterSelf = new List<CharacterBase>();

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
    }


    public override void OnTurnEnd()// 턴이 끝날 때
    {
        //힐량 = 잃은 체력(=최대체력에서 현재 체력을 뺀 값) * 10% ( 10 / 100 )
        if (characterSelf.Count <= 0)
        {
            characterSelf.Add(character);
        }
        int healAmount = (int)((character.health.MaxHealth - character.health.CurHealth) * (coefficient["healRate"] / coefficient["denominator"])); //잃은 체력을 구한다
        BattleManager.Instance.ExtraSkillHeal(character, healAmount, characterSelf, BattleKeyWords.AttackDamageType.Extra);//잃은 체력의 10% 회복
        //character.TakeHealByInt(ref healAmount); 
    }
}

