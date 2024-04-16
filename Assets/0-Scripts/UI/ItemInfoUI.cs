using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoUI : UIBase
{
    private ItemSO item;

    private enum Texts
    {
        NameText,
        DescriptionText,
        NumberText
    }

    private enum Images
    {
        ItemImage
    }

    private enum Buttons
    {
        BackImage,
        UseButton
    }

    public void Init(ItemSO item)
    {
        this.item = item;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.UseButton).onClick.AddListener(OnClickUseButton);
        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);

        GetText((int)Texts.NameText).text = item.itemName;
        GetText((int)Texts.DescriptionText).text = item.itemDescription;
        GetText((int)Texts.NumberText).text = Managers.AccountData.inventory[item.id].ToString();

        // GetImage((int)Images.ItemImage).sprite = ;
    }

    private void OnClickUseButton()
    {

    }

    private void CloseUI()
    {
        Managers.UI.CloseUI(this);
    }
}
