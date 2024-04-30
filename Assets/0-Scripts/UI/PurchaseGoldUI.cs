using UnityEngine;

public class PurchaseGoldUI : UIBase
{
    private int purchaseCount = 1;
    private int reduceDiamond = 50;
    private int addGold = 100000;

    private enum Texts
    {
        GoldQuantity,
        DiamondQuantity
    }

    private enum Buttons
    {
        BackImage,
        CancelButton,
        CloseButton,
        PurchaseButton,
        IncreButton,
        DecreButton,
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
        GetButton((int)Buttons.PurchaseButton).onClick.AddListener(PurchaseGold);
        GetButton((int)Buttons.IncreButton).onClick.AddListener(OnClickIncreButton);
        GetButton((int)Buttons.DecreButton).onClick.AddListener(OnClickDecreButton);
        GetButton((int)Buttons.MaxButton).onClick.AddListener(OnClickMaxButton);
        GetButton((int)Buttons.MinButton).onClick.AddListener(OnClickMinButton);
    }

    private void OnClickIncreButton()
    {
        purchaseCount++;
        TextUpdate();
    }
    private void OnClickDecreButton()
    {
        if (purchaseCount == 1)
        {
            return;
        }
        purchaseCount--;
        TextUpdate();
    }
    private void OnClickMaxButton()
    {
        purchaseCount = Managers.AccountData.playerData.Diamond / reduceDiamond;
        if (purchaseCount == 0)
        {
            purchaseCount = 1;
        }
        TextUpdate();
    }
    private void OnClickMinButton()
    {
        purchaseCount = 1;
        TextUpdate();
    }

    private void TextUpdate()
    {
        GetText((int)Texts.GoldQuantity).text = (purchaseCount * addGold).ToString();
        GetText((int)Texts.DiamondQuantity).text = (purchaseCount * reduceDiamond).ToString();
    }

    private void PurchaseGold()
    {
        if (Managers.AccountData.playerData.ReduceDiamond(purchaseCount * reduceDiamond) == false)
        {
            Managers.UI.ShowUI<WarningUI>().Init("다이아가 부족합니다.");
            return;
        }
        Managers.AccountData.playerData.AddGold(purchaseCount * addGold);

        CloseUI();
    }

    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }

}
