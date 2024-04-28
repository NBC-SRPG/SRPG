using UnityEngine;

public class RewardIconUI : UIBase
{
    private enum Texts
    {
        RewardText
    }

    private enum Images
    {
        RewardImage
    }

    public void Init(string rewardText, string imageId)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.RewardText).text = rewardText;
        GetImage((int)Images.RewardImage).sprite = Managers.Resource.Load<Sprite>(imageId);
    }

    public void Init(string rewardText, Sprite imageSprite)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.RewardText).text = rewardText;
        GetImage((int)Images.RewardImage).sprite = imageSprite;
    }
}