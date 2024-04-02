using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MailData", fileName = "Mail_")]
public class MailSO : ScriptableObject
{
    public string id; // 메일 ID
    public string title; // 메일 제목
    public Dictionary<string, int> rewards; // 수령품 (아이템 ID와 수량)
    public string sender; // 보낸 사람
    public DateTime dateSent; // 보낸 날짜
    public TimeSpan remainingTime; // 남은 기간 (만료까지 남은 시간)
}
