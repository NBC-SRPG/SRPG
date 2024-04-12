using UnityEngine;

public class ItemEntryUI : UIBase
{
    private ItemData item;
    private enum Texts
    {
        NumberText
    }

    private enum Images
    {
        ItemImage
    }

    private enum Buttons
    {
        SelectButton
    }

    public void Init(ItemData item)
    {
        this.item = item;
        Debug.Log("ItemEntryUIInit");
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetText((int)Texts.NumberText).text = Managers.AccountData.inventory[item.item_Id].ToString();
        Debug.Log("ItemEntryUIInit1");
        GetButton((int)Buttons.SelectButton).onClick.AddListener(OnClickSelectButton);
        Debug.Log("ItemEntryUIInit2");
    }

    private void OnClickSelectButton()
    {
        Debug.Log("OnClickSelectButton");
        ItemInfoUI ui = Managers.UI.ShowUI<ItemInfoUI>();
        ui.Init(item);
    }
}
