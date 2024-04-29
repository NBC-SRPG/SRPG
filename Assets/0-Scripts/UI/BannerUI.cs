public class BannerUI : UIBase
{
    private enum Texts
    {

    }

    private enum Images
    {
        BannerImage
    }

    private enum Buttons
    {
        BannerButton
    }

    public void Init(GachaSO gachaSO)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));


        
        GetImage((int)Images.BannerImage).sprite = gachaSO.banner;
        GetButton((int)Buttons.BannerButton).onClick.AddListener(() => OnClickBannerButton(gachaSO));
    }

    private void OnClickBannerButton(GachaSO gachaSO)
    {
        Managers.UI.PeekUI<GachaUI>().UpdateGachaInfoUI(gachaSO);
    }
}
