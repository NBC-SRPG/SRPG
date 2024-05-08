using System;
using UnityEngine;

public class MailEntryUI : UIBase
{
    private MailSO mailSO; // 메일 정보
    private TimeSpan expiration; // 만료까지 남은 시간

    private enum Texts
    {
        MailTitleText,
        MailDateText,
        MailRemainingTimeText
    }
    
    private enum Buttons
    {
        ReceiveButton
    }

    private enum GameObjects
    {
        Content
    }

    public void Init(MailSO mailSO)
    {
        this.mailSO = mailSO;

        expiration = mailSO.dateSent.AddDays(mailSO.expiration) - DateTime.Now;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        Debug.Log(mailSO.title);
        GetText((int)Texts.MailTitleText).text = mailSO.title;
        GetText((int)Texts.MailDateText).text = "받은 날짜 " + mailSO.dateSent.ToString("yyyy-MM-dd");
        GetText((int)Texts.MailRemainingTimeText).text = "수령 기한 " + GetExpiration();

        // ap 보상 아이콘 생성
        if (mailSO.ap > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<RewardIconUI>().Init(mailSO.ap.ToString(), "AP");
        }
        // 골드 보상 아이콘 생성
        if (mailSO.gold > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<RewardIconUI>().Init(mailSO.gold.ToString(), "Gold");
        }
        // 다이아 보상 아이콘 생성
        if (mailSO.diamond > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.Content).transform);

            go.GetComponent<RewardIconUI>().Init(mailSO.diamond.ToString(), "Diamond");
        }
        // 아이템 보상 아이콘 생성
        foreach (var item in mailSO.rewards)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.Content).transform);
            Utility.Id2SO<ItemSO>(item.Key, (result) =>
            {
                go.GetComponent<RewardIconUI>().Init(item.Value.ToString(), (result as ItemSO).icon);
            });

        }

        GetButton((int)Buttons.ReceiveButton).onClick.AddListener(OnClickReceiveButton);
    }

    public void OnClickReceiveButton()
    {
        Debug.Log("OnClickReceiveButton");

        if (mailSO.isExpired())
        {
            Managers.UI.ShowUI<WarningUI>().Init("수령기간이 만료되었습니다.");
            DeleteMailEntry();
        }
        else
        {
            mailSO.GetRewards();

            Managers.UI.ShowUI<WarningUI>().Init("수령 완료");
            DeleteMailEntry();
        }
    }

    private void DeleteMailEntry()
    {
        Managers.AccountData.DeleteMail(mailSO);
        Managers.UI.FindUI<MailUI>().UpdateReceiveText();

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
