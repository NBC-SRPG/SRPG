using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MailUI : UIBase
{
    private List<MailEntryUI> mails = new List<MailEntryUI>();
    private enum Texts
    {
        WaitingReceiveText
    }
    private enum Buttons
    {
        AllReceiveButton,
        BackButton
    }

    private enum GameObjects
    {
        Content
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.AllReceiveButton).onClick.AddListener(OnClickAllReceiveButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetText((int)Texts.WaitingReceiveText).text = $"수령대기 {Managers.AccountData.mailBox.Count}건";

        // 테스트 데이터
        foreach (var mail in Managers.AccountData.mailBox)
        {
            GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/MailEntryUI");
            go.GetComponent<MailEntryUI>().mailSO = mail;
            mails.Add(Managers.Resource.Instantiate(go, GetObject((int)GameObjects.Content).transform).GetComponent<MailEntryUI>());
        }
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    private void OnClickAllReceiveButton()
    {
        Debug.Log("OnClickAllReceiveButton");

        // 모든 메일 순회하며 아이템 인벤토리에 수령
        foreach (var mail in mails)
        {
            mail.OnClickReceiveButton();
        }
    }

    public void ReceiveTextUpdate()
    {
        GetText((int)Texts.WaitingReceiveText).text = $"수령대기 {Managers.AccountData.mailBox.Count}건";
    }
}
