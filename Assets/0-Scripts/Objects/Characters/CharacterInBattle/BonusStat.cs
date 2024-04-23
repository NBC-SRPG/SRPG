using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStat
{
    public int ExtraHealth {  get; set; }
    public float ExtraAtk { get; set; }
    public float ExtraDefend { get; set; }
    public int ExtraMov {  get; set; }
    public int EXCritRate {  get; set; }
    public int EXCritDMG {  get; set; }
    public float PenetrateDef { get; set; }// % 방어 관통
    public float EnhancedDmg { get; set; }
    public float ReducedDmg { get; set; }

    public void AddBonusStat(BonusStat stat)
    {
        ExtraHealth += stat.ExtraHealth;
        ExtraAtk *= 1f+stat.ExtraAtk;
        ExtraDefend *= 1f+stat.ExtraDefend;
        ExtraMov += stat.ExtraMov;
        EXCritRate += stat.EXCritRate;
        EXCritDMG += stat.EXCritDMG;
        PenetrateDef *= 1f-stat.PenetrateDef; // 초기값 0
        EnhancedDmg *= 1f+stat.EnhancedDmg;
        ReducedDmg *= 1f-stat.ReducedDmg;
    }


}

public class ShieldStat
{
    public int Shield { get; set; }
    public int duration {  get; set; }
}
