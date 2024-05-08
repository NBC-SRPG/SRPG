using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassAbility_102 : PassiveLogic
{   
    //학살자
    BonusStat stat_Class102;
    public override void init(CharacterBase character)// 패시브 소유자 설정
    {
        this.character = character;
        stat_Class102 = new BonusStat();
        stat_Class102.ExtraAtk = (float)(coefficient["increaseAtk"]) / coefficient["denominator"];
        stat_Class102.EXCritRate = coefficient["increaseCrtRate"];
        
    }
    public override void OnStageStart()
    {
        character.tempBonusStat.AddBonusStat(stat_Class102);
    }
    
}