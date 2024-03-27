using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailEntryUI : UIBase
{
    private MailSO mailSO; // 메일 정보
    private DateTime expireDate; // 메일 만료 날짜
    private string formattedTime; // 날짜 -> 일 시 분 포맷 변경용
    private int curMin; // 현재 시간
    private int lastMin = -1; // 이전 시간 (현재 시간과 다르면 남은 시간 업데이트용)

    private enum Texts
    {
        MailTitleText,
        MailSenderText,
        MailDateText,
        MailRemainingTimeText
    }
    
    private enum Buttons
    {
        ReceiveButton
    }

    private enum Images
    {
        MailItemImage
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        // 메일의 남은 날짜 업데이트
        mailSO.remainingTime = expireDate - DateTime.Now;

        // 현재 남은 시간 업데이트
        curMin = mailSO.remainingTime.Days * 24 * 60 + mailSO.remainingTime.Hours * 60 + mailSO.remainingTime.Minutes;

        // 이전 남은 시간과 다르다면 (분이 바뀌었다면)
        if (lastMin != curMin)
        {
            // 남은 날짜 일 시 분 포맷에 맞춰 업데이트
            formattedTime = $"{mailSO.remainingTime.Days}일 {mailSO.remainingTime.Hours}시간 {mailSO.remainingTime.Minutes}분";
            GetText((int)Texts.MailRemainingTimeText).text = formattedTime;
            lastMin = curMin;
        }
    }

    private void Init()
    {
        expireDate = mailSO.dateSent.AddDays(14);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetText((int)Texts.MailTitleText).text = mailSO.title;
        GetText((int)Texts.MailSenderText).text = mailSO.sender;
        GetText((int)Texts.MailDateText).text = mailSO.dateSent.ToString();

        // TODO
        // MailItemImage를 아이템 데이터의 아이콘 이미지로 업데이트

        GetButton((int)Buttons.ReceiveButton).onClick.AddListener(OnClickReceiveButton);
    }

    public void SetMailSO(MailSO mailSO)
    {
        this.mailSO = mailSO;
    }

    public void OnClickReceiveButton()
    {
        Debug.Log("OnClickReceiveButton");

        // TODO
        // 아이템 인벤토리에 수령
        // 수령 완료 팝업
        Managers.AccountData.mailBox.Remove(mailSO);
        Managers.UI.PeekUI<MailUI>().UpdateReceiveText();
        Destroy(gameObject);
    }
}
