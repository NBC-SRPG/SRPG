using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : UIBase
{
    private enum Texts
    {
        WarningText
    }

    private enum Buttons
    {
        BackImage,
        WarningButton
    }

    public void Init(string text)
    {

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetText((int)Texts.WarningText).text = text;

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.WarningButton).onClick.AddListener(CloseUI);

    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
