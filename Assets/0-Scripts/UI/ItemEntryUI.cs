using UnityEngine;

public class ItemEntryUI : UIBase
{
    private ItemSO item;
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

    public void Init(ItemSO item)
    {
        this.item = item;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetText((int)Texts.NumberText).text = Managers.AccountData.inventory[item.id].ToString();
        GetButton((int)Buttons.SelectButton).onClick.AddListener(OnClickSelectButton);
    }

    private void OnClickSelectButton()
    {
        Debug.Log("OnClickSelectButton");
        ItemInfoUI ui = Managers.UI.ShowUI<ItemInfoUI>();
        ui.Init(item);
    }
}
