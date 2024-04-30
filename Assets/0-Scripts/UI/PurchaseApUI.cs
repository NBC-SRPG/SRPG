using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseApUI : UIBase
{
    public int exchangeCount = 0;
    private enum Texts
    {
        ExchangeCount,
        DiamondCount,
        ApCount
    }

    private enum Buttons
    {
        BackImage,
        CancelButton,
        CloseButton,
        PurchaseButton,
        AddButton,
        ReduceButton
    }

    public void Init()
    {

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CancelButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.PurchaseButton).onClick.AddListener(PurchaseAp);
        GetButton((int)Buttons.AddButton).onClick.AddListener(OnClickAddButton);
        GetButton((int)Buttons.ReduceButton).onClick.AddListener(OnClickReduceButton);
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

    private void OnClickReduceButton()
    {
        if (exchangeCount == 0)
        {
            return;
        }

        exchangeCount--;

        GetText((int)Texts.ExchangeCount).text = exchangeCount.ToString();
        GetText((int)Texts.DiamondCount).text = $"{exchangeCount * 50}";
        GetText((int)Texts.ApCount).text = $"{exchangeCount * 100}";
    }
    private void OnClickAddButton()
    {
        if ((exchangeCount + 1) * 50 > Managers.AccountData.playerData.Diamond)
        {
            return;
        }

        exchangeCount++;

        GetText((int)Texts.ExchangeCount).text = exchangeCount.ToString();
        GetText((int)Texts.DiamondCount).text = $"{exchangeCount * 50}";
        GetText((int)Texts.ApCount).text = $"{exchangeCount * 100}";
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
