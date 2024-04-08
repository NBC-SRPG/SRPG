using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : UIBase
{
    private bool isInit = false;

    private enum Texts
    {
        WarningText
    }

    private enum Buttons
    {
        BackImage,
        WarningButton
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (isInit)
        {
            return;
        }

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.WarningButton).onClick.AddListener(CloseUI);

        isInit = true;
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

    // 경고 UI의 텍스트를 바꿔주는 함수
    // 경고 UI를 생성 후 바로 텍스트를 바꾸는 경우 Init보다 먼저 호출이 되어 널 오류가 뜰 가능성이 있음
    // isInit bool값으로 Init이 되었는지 체크 후 안되었다면 Init후에 텍스트 변경
    public void SetText(string text)
    {
        if (!isInit)
        {
            Init();
        }
        GetText((int)Texts.WarningText).text = text;
    }
}
