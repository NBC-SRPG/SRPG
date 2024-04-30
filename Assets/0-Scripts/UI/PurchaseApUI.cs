using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseApUI : UIBase
{
    private enum Texts
    {
        
    }

    private enum Buttons
    {
        BackImage,
        CancelButton,
        CloseButton,
        PurchaseButton
    }

    public void Init()
    {

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CancelButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.PurchaseButton).onClick.AddListener(PurchaseAp);
    }

    private void PurchaseAp()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(50) == false)
        {
            Managers.UI.ShowUI<WarningUI>().Init("다이아가 부족합니다.");

            return;
        }
        Managers.AccountData.playerData.AddAP(100);

        CloseUI();
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
