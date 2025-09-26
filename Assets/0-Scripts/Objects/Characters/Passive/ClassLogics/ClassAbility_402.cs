using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_402 : PassiveLogic
{
    //대마법사
    BonusStat stat_Class402;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class402 = new BonusStat();
        stat_Class402.EnhancedDmg = (float)(coefficient["increaseDmg"]) / coefficient["denominator"];
        
    }


    public override void OnUseSkill(List<CharacterBase> targets)// 스킬 실제 사용
    {
        //스킬 사용시 일시적으로 데미지 보너스를 얻는다.
        stat_Class402.EnhancedDmg = (float)(coefficient["increaseDmg"]) / coefficient["denominator"];
        character.tempBonusStat.AddBonusStat(stat_Class402);
    }
    public override void OnEndSkill(List<CharacterBase> target)// 스킬 사용 종료 시
    {
        character.tempBonusStat.RemoveBonusStat(stat_Class402);
    }

}

