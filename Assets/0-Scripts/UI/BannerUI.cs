public class BannerUI : UIBase
{
    private enum Texts
    {
        BannerText
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

        if (gachaSO.gachaType == Constants.GachaType.Common)
        {
            GetText((int)Texts.BannerText).text = "통상 계약";
        }
        else
        {
            GetText((int)Texts.BannerText).text = "픽업 계약";
        }
        
        GetImage((int)Images.BannerImage).sprite = gachaSO.banner;
        GetButton((int)Buttons.BannerButton).onClick.AddListener(() => OnClickBannerButton(gachaSO));
    }

    private void OnClickBannerButton(GachaSO gachaSO)
    {
        Managers.UI.PeekUI<GachaUI>().UpdateGachaInfoUI(gachaSO);
    }
}
