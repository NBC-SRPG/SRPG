using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusStat
{
    public int ExtraHealth {  get; set; }
    public int ExtraAtk { get; set; }
    public int ExtraDefend { get; set; }
    public int ExtraMov {  get; set; }

    public void AddStat(BonusStat stat)
    {
        ExtraHealth += stat.ExtraHealth;
        ExtraAtk += stat.ExtraAtk;
        ExtraDefend += stat.ExtraDefend;
        ExtraMov += stat.ExtraMov;
    }
}
