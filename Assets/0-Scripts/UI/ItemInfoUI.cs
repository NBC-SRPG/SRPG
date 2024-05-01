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
        NumberText,
        LocationText
    }

    private enum Images
    {
        ItemImage
    }

    private enum Buttons
    {
        BackImage,
        UseButton,
        CloseButton
    }

    public void Init(ItemSO item)
    {
        this.item = item;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.UseButton).onClick.AddListener(OnClickUseButton);
        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);

        GetText((int)Texts.NameText).text = item.itemName;
        GetText((int)Texts.DescriptionText).text = item.itemDescription;
        GetText((int)Texts.NumberText).text = "보유 수량 : " + Managers.AccountData.inventory[item.id].ToString();
        GetImage((int)Images.ItemImage).sprite = item.icon;

        if (item.values.ContainsKey("location"))
        {
            GetText((int)Texts.LocationText).text = $"획득장소 : {Utility.Id2SOWait<StageSO>(item.values["location"]).stageNumber}";
        }
        else
        {
            GetText((int)Texts.LocationText).gameObject.SetActive(false);
        }

        if (item.itemType != Constants.ItemType.Consumable)
        {
            GetButton((int)Buttons.UseButton).transform.gameObject.SetActive(false);
        }
    }

    private void OnClickUseButton()
    {

    }

    private void CloseUI()
    {
        Managers.UI.CloseUI(this);
    }
}
