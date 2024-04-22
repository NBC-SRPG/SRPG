using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MailEntryUI : UIBase
{
    private MailSO mailSO; // 메일 정보
    private TimeSpan expiration; // 만료까지 남은 시간

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

    private void Init()
    {
        expiration = mailSO.dateSent.AddDays(mailSO.expiration) - DateTime.Now;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        Debug.Log(mailSO.title);
        GetText((int)Texts.MailTitleText).text = mailSO.title;
        GetText((int)Texts.MailDateText).text = "받은 날짜 " + mailSO.dateSent.ToString("yyyy-MM-dd");
        GetText((int)Texts.MailRemainingTimeText).text = "수령 기한 " + GetExpiration();

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

        if (mailSO.isExpired())
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("수령기간이 만료되었습니다.");
            DeleteMailEntry();
        }
        else
        {
            mailSO.GetRewards();

            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("수령 완료");
            DeleteMailEntry();
        }
    }

    private void DeleteMailEntry()
    {
        Managers.AccountData.DeleteMail(mailSO);
        Managers.UI.PeekUI<MailUI>().UpdateReceiveText();

        Destroy(gameObject);
    }

    private string GetExpiration()
    {
        if (expiration > TimeSpan.FromHours(24))
        {
            return expiration.ToString("%d") + "일";
        }
        else if (expiration > TimeSpan.FromHours(1))
        {
            return expiration.ToString("%h") + "시간";
        }
        else return "1시간 미만";
    }
}
