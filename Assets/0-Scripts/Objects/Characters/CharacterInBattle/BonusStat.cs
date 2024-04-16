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
    public int EnhancedDmg {  get; set; }
    public float ReducedDmg { get; set; }

    public void AddBonusStat(BonusStat stat)
    {
        ExtraHealth += stat.ExtraHealth;
        ExtraAtk *= ((100f + stat.ExtraAtk) / 100f);
        ExtraDefend *= ((100f + stat.ExtraDefend) / 100f);
        ExtraMov += stat.ExtraMov;
        EXCritRate += stat.EXCritRate;
        EXCritDMG += stat.EXCritDMG;
        PenetrateDef *= ((100f + stat.PenetrateDef) / 100f);
        EnhancedDmg += stat.EnhancedDmg;
        ReducedDmg *= ((100f + stat.ReducedDmg) / 100f);
    }

    public void AddDecreaseStat(BonusStat stat)
    {
        ExtraHealth += stat.ExtraHealth;
        ExtraAtk += stat.ExtraAtk;
        ExtraDefend += stat.ExtraDefend;
        ExtraMov += stat.ExtraMov;
        EXCritRate += stat.EXCritRate;
        EXCritDMG += stat.EXCritDMG;
    }

}

public class ShieldStat
{
    public int Shield { get; set; }

}
