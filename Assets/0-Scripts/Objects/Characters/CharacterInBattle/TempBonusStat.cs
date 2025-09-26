using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBonusStat
{
    public List<BonusStat> statList;

    public TempBonusStat()
    {
        statList = new List<BonusStat>();
    }

    public BonusStat GetTempStat()
    {
        BonusStat stat = new BonusStat
        {
            ExtraAtk = 1f,
            ExtraDefend = 1f,
            PenetrateDef = 1f,
            EnhancedDmg = 1f,
            ReducedDmg = 1f,
        };// 곱연산인 변수들은 초기값 1로

        foreach (BonusStat bonusStat in statList)
        {
            stat.AddBonusStat(bonusStat);
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
