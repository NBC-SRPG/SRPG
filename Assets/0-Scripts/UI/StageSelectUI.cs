using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : UIBase
{
    private List<StageSO> stages = new();
    private int numberOfStars;
    private enum Texts
    {
        ChapterNameText,
        StageDescriptionText,
        StageStarText
    }

    private enum Images
    {
        StageImage
    }

    private enum Buttons
    {
        BackButton,
        HomeButton
    }

    private enum GameObjects
    {
        Content
    }

    public void Init(int chapterId)
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton (typeof(Buttons));
        BindObject(typeof(GameObjects));

        // TODO
        // 챕터 이름, 설명, 이미지는 어디에??
        ChapterSO chapter = Utility.Id2SOWait<ChapterSO>(chapterId);
        GetText((int)Texts.ChapterNameText).text = $"{chapter.chapterName}";
        GetText((int)Texts.StageDescriptionText).text = $"{chapter.description}";

        GetImage((int)Images.StageImage).sprite = chapter.chapterImage;

        // TODO
        // Chapter에 스테이지가 몇개 있는지는 어떻게 알지?? 5개 고정?
        foreach (var stageId in chapter.stageList)
        {
            GameObject go = Managers.Resource.Load<GameObject>("Prefabs/UI/StageEntryUI");
            GameObject ui = Managers.Resource.Instantiate(go, GetObject((int)GameObjects.Content).transform);
            ui.GetComponent<StageEntryUI>().Init(stageId);

            int stageClearStars;

            if (!Managers.AccountData.stageClearData.TryGetValue(stageId, out stageClearStars))
            {
                stageClearStars = 0;
            }

            numberOfStars += stageClearStars;
        }

        // TODO
        // 15 -> 스테이지 개수 * 3
        GetText((int)Texts.StageStarText).text = $"{numberOfStars} / 15";

        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
    }

    private void OnClickBackButton()
    {
        Managers.UI.CloseUI(this);
    }

}
