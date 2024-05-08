using UnityEngine;

public class PurchaseApUI : UIBase
{
    public int exchangeCount = 0;
    private int reduceDiamond = 50;
    private int addAp = 100;

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
        ReduceButton,
        MaxButton,
        MinButton
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
        GetButton((int)Buttons.MaxButton).onClick.AddListener(OnClickMaxButton);
        GetButton((int)Buttons.MinButton).onClick.AddListener(OnClickMinButton);
    }

    private void PurchaseAp()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(reduceDiamond * exchangeCount) == false)
        {
            Managers.UI.ShowUI<WarningUI>().Init("다이아가 부족합니다.");

            return;
        }
        Managers.AccountData.playerData.AddAP(addAp * exchangeCount);

        CloseUI();
    }

    private void OnClickReduceButton()
    {
        if (exchangeCount == 0)
        {
            return;
        }

        exchangeCount--;

        TextUpdate();
    }
    private void OnClickAddButton()
    {
        if ((exchangeCount + 1) * reduceDiamond > Managers.AccountData.playerData.Diamond)
        {
            return;
        }

        exchangeCount++;

        TextUpdate();
    }
    private void OnClickMaxButton()
    {
        exchangeCount = Managers.AccountData.playerData.Diamond / reduceDiamond;

        TextUpdate();
    }
    private void OnClickMinButton()
    {
        exchangeCount = 0;

        TextUpdate();
    }
    private void TextUpdate()
    {
        GetText((int)Texts.ExchangeCount).text = exchangeCount.ToString();
        GetText((int)Texts.DiamondCount).text = (reduceDiamond * exchangeCount).ToString();
        GetText((int)Texts.ApCount).text = (addAp * exchangeCount).ToString();
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
