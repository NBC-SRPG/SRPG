using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MailData", fileName = "MailSO_")]
public class MailSO : ScriptableObject
{
    public string key;
    public string title; // 메일 제목
    public Dictionary<int, int> rewards = new(); // 수령품 (아이템 ID와 수량)
    public int ap; // 보상 ap
    public int gold; // 보상 골드
    public int diamond; // 보상 다이아
    public DateTime dateSent; // 보낸 날짜
    public int expiration = 7; // 유효 기간(기본 7일)
    //public TimeSpan remainingTime; // 남은 기간 (만료까지 남은 시간)

    public bool isExpired()
    {
        return dateSent.AddDays(expiration) > DateTime.Now ? false : true;
    }

    public void GetRewards()
    {
        Managers.AccountData.playerData.AddAP(ap);
        Managers.AccountData.playerData.AddGold(gold);
        Managers.AccountData.playerData.AddDiamond(diamond);

        foreach (var reward in rewards)
        {
            Managers.AccountData.AcquireItems(reward.Key, reward.Value);
        }
    }
}