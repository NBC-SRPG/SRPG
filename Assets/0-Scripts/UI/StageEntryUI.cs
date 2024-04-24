using UnityEngine;

public class StageEntryUI : UIBase
{
    private StageSO stageSO;
    private int numberOfStars;

    private enum Texts
    {
        StageNumText,
        StageTitleText
    }

    private enum Buttons
    {
        StageClearButton,
        StageTitleText
    }

    private enum GameObjects
    {
        StageStar
    }

    public void Init(string stage)
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        Utility.Stage2SO<StageSO>(stage, (retulst) =>
        {
            stageSO = (StageSO)retulst;

            GetText((int)Texts.StageTitleText).text = stageSO.stageName;
            GetButton((int)Buttons.StageTitleText).onClick.AddListener(() => OnClickStageTitle(stageSO));
        });

        InitStar(stage);

        GetText((int)Texts.StageNumText).text = stage;

        if (numberOfStars != 3)
        {
            GetButton((int)Buttons.StageClearButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.StageClearButton).onClick.AddListener(OnClickStageClearButton);
        }
    }

    private void InitStar(string stage)
    {
        if (!Managers.AccountData.stageClearData.TryGetValue(stage, out numberOfStars))
        {
            numberOfStars = 0; // 키가 없을 때의 기본 값
        }

        float starWidth = 40f; // 별 이미지의 너비
        float spacing = 10f; // 별 사이의 간격

        // 별 이미지들의 총 너비 계산
        float totalWidth = numberOfStars * starWidth + (numberOfStars - 1) * spacing;

        // 첫 번째 별 이미지의 시작 위치 계산
        float startX = -(totalWidth / 2) + (starWidth / 2);

        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)GameObjects.StageStar).transform);
            RectTransform rt = star.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(startX + i * (starWidth + spacing), 0);
            rt.sizeDelta = new Vector2(40f, 40f);
        }
    }
    
    private void OnClickStageTitle(StageSO stage)
    {
        StageInfoUI ui = Managers.UI.ShowUI<StageInfoUI>();
        ui.Init(stage);
    }

    private void OnClickStageClearButton()
    {
        Debug.Log("OnClickStageClearButton");

        // TODO
        // 스테이지 소탕 기능 추가
    }
}
