using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_931: PassiveLogic
{
    //"정밀 분석"
    //크리티컬 데미지가 30%증가하고, 적의 방어력을 20% 무시한다.
    //에오스의 3-1 특성


    BonusStat stat_931;


    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;

    }

    public override void OnStageStart()
    {
        stat_931 = new BonusStat();
        stat_931.EXCritDMG = (coefficient["crtDmgIncreaseRate"]);
        stat_931.PenetrateDef = (float)(coefficient["defPenetrateRate"]) / coefficient["denominator"];
        character.tempBonusStat.AddBonusStat(stat_931);
    }
}

