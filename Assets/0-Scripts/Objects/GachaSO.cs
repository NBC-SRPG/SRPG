using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GachaData", fileName = "GachaSO_")]
public class GachaSO : ScriptableObject
{
    public Constants.GachaType gachaType;
    public int id;  // 가챠정보 ID = 픽업캐릭터ID
    public string pickUpcharacterName;  // 픽업 캐릭터 이름
    public int tableId; // 확률테이블 키값. ID + 10000
    public DateTime startDate; // 시작 날짜
    public int expiration = 7; // 픽업 기간
    public Sprite banner;   // 배너 이미지
    public Sprite gachaImage;   // 가챠 정보에 들어갈 이미지

    public bool isExpired()
    {
        return startDate.AddDays(expiration) > DateTime.Now ? false : true;
    }

    public string GetRemainingTimeText()
    {
        if (!isExpired())
        {
            TimeSpan remainingTime = startDate.AddDays(expiration) - DateTime.Now;
            return string.Format($"{remainingTime.Days}일 {remainingTime.Hours}시간 {remainingTime.Minutes}분");
        }

        return "";
    }
}