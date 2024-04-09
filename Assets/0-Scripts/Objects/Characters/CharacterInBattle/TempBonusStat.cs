using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBonusStat
{
    private List<BonusStat> statList;

    public TempBonusStat()
    {
        statList = new List<BonusStat>();
    }

    public BonusStat GetTempStat()
    {
        BonusStat stat = new BonusStat();

        foreach(BonusStat bonusStat in statList)
        {
            stat.AddStat(bonusStat);
        }

        return stat;
    }

    public BonusStat FindBonusStat(BonusStat stat)
    {
        foreach(BonusStat bonusStat in statList)
        {
            if( bonusStat == stat)
            {
                return bonusStat;
            }
        }

        return null;
    }

    public void AddBonusStat(BonusStat bonusStat)
    {
        statList.Add(bonusStat);
    }

    public void RemoveBonusStat(BonusStat bonusStat)
    {
        statList.Remove(bonusStat);
    }

    public void ClearAllStat()
    {
        statList.Clear();
    }
}
