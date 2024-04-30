using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseGoldUI : UIBase
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
        GetButton((int)Buttons.PurchaseButton).onClick.AddListener(PurchaseGold);
    }

    private void PurchaseGold()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(50) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.Init("다이아가 부족합니다.");
            return;
        }
        Managers.AccountData.playerData.AddGold(100000);

        CloseUI();
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
