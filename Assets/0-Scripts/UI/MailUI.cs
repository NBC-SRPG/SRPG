using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MailUI : UIBase
{
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
            GameObject go = Managers.Resource.Instantiate("UI/MailEntryUI", GetObject((int)GameObjects.Content).transform);
            go.GetComponent<MailEntryUI>().SetMailSO(mail);
        }
    }

    private void OnClickBackButton()
    {
        Debug.Log("OnClickBackButton");
        // TODO 버튼 클릭 효과음
        Managers.UI.CloseUI(this);
    }

    // 모두 수령
    private void OnClickAllReceiveButton()
    {
        Debug.Log("OnClickAllReceiveButton");
        // Content 오브젝트 아래에 있는 메일을 순회하며 수령
        foreach (Transform child in GetObject((int)GameObjects.Content).transform)
        {
            MailEntryUI mailEntryUI = child.GetComponent<MailEntryUI>();
            mailEntryUI.OnClickReceiveButton();
        }
    }

    // 수령 후 남은 메일 건수 텍스트 업데이트
    public void UpdateReceiveText()
    {
        GetText((int)Texts.WaitingReceiveText).text = $"수령대기 {Managers.AccountData.mailBox.Count}건";
    }
}
