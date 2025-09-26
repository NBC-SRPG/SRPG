using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_109 : PassiveLogic
{
    //ID 009. 에오스 E-03
    //패시브 스킬
    //E03는 보조무기를 메인무기와 동일하게 장착한다.
    //추가로 크리티컬확률이 10% 증가한다

    BonusStat stat_009;

    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_009 = new BonusStat();
        stat_009.EXCritRate = coefficient["increaseCrtRate"];
        
    }

    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_009);
    }
}

