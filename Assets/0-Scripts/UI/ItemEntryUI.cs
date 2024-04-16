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

        GetButton((int)Buttons.SelectButton).onClick.AddListener(OnClickSelectButton);
    }

    private void OnClickSelectButton()
    {
        ItemInfoUI ui = Managers.UI.ShowUI<ItemInfoUI>();
        ui.Init(item);
    }
}
