using System;
using UnityEngine;

public class StageInfoUI : UIBase
{
    private bool isInit = false;
    private int stageClearCount = 0;

    private enum Texts
    {
        StageNumText,
        StageNameText,
        StageLevelText,
        ClearCountText,
        Goal
    }

    private enum Images
    {
        Party1Image,
        Party2Image,
        Party3Image,
        Party4Image,
        Party5Image
    }

    private enum Buttons
    {
        FormationButton,
        ClearDecreButton,
        ClearIncreButton,
        ClearMaxButton,
        ClearButton,
        EnterButton,
        EnemyInfoButton,
        BackButton
    }

    private void OnEnable()
    {
        // 편성에 갔다가 다시 왔을 때 이미지 업데이트
        if (isInit)
        {
            InitImage();
        }
    }

    public void Init(StageSO stage)
    {
        if (isInit == true)
        {
            return;
        }

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        // TODO
        // SO에 스테이지 번호 없음
        // 매개변수로 String을 받고 여기에서 StageSO 불러오기? -> 그럼 StageEntryUI에서도 SO로드하고 여기에서도 SO를 로드하는 형태가 됨
        // SO의 이름에서 숫자 추출?
        /*
        string pattern = @"\d+";

        // Regex.Matches를 사용하여 모든 숫자 찾기
        //MatchCollection matches = Regex.Matches(stage.name, pattern);

        if (matches.Count > 0)
        {
            // 숫자들을 추출하여 배열에 저장
            string[] numbers = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                numbers[i] = matches[i].Value;
            }

            // 배열의 원소를 "-"로 연결
            string result = string.Join("-", numbers);
            GetText((int)Texts.StageNumText).text = result;
        }
        */
        GetText((int)Texts.StageNumText).text = stage.stageNumber;
        GetText((int)Texts.StageNameText).text = $"{stage.stageName}";
        GetText((int)Texts.StageLevelText).text = $"권장레벨 {stage.recommendLevel}";

        // TODO
        // 목표를 달성 했는지 어떻게 알지??
        GetText((int)Texts.Goal).text += "\n☆";
        GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(0);
        GetText((int)Texts.Goal).text += "\n☆";
        GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(1);
        GetText((int)Texts.Goal).text += "\n☆";
        GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(2);

        GetButton((int)Buttons.FormationButton).onClick.AddListener(OnClickFormationButton);
        GetButton((int)Buttons.ClearDecreButton).onClick.AddListener(OnClickClearDecreButton);
        GetButton((int)Buttons.ClearIncreButton).onClick.AddListener(OnClickClearIncreButton);
        GetButton((int)Buttons.ClearMaxButton).onClick.AddListener(OnClickClearMaxButton);
        GetButton((int)Buttons.ClearButton).onClick.AddListener(OnClickClearButton);
        GetButton((int)Buttons.EnterButton).onClick.AddListener(OnClickEnterButton);
        GetButton((int)Buttons.EnemyInfoButton).onClick.AddListener(OnClickEnemyInfoButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        InitImage();

        isInit = true;

        Managers.GameManager.thisStage = stage;
    }

    private void InitImage()
    {
        for (int i = 0; i < 5; i++)
        {
            Images partyImageEnum = (Images)Enum.Parse(typeof(Images), $"Party{i + 1}Image");
            int characterId = Managers.AccountData.formationData[Constants.presetIndex].characterId[i];

            if (characterId == 0)
            {
                GetImage((int)partyImageEnum).sprite = null;
                continue;
            }

            Sprite icon = Managers.AccountData.characterData[characterId].SO.icon;
            GetImage((int)partyImageEnum).sprite = icon;
        }
    }

    private void OnClickClearDecreButton()
    {
        if (stageClearCount == 0)
        {
            return;
        }

        stageClearCount--;
        
        // TODO
        // 한 판당 소모 AP는 어디에??
        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * 5}";
    }
    private void OnClickClearIncreButton()
    {
        if ((stageClearCount + 1) * 5 > Managers.AccountData.playerData.Ap)
        {
            return;
        }

        stageClearCount++;

        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * 5}";
    }

    private void OnClickClearMaxButton()
    {
        int maxCount = Managers.AccountData.playerData.Ap / 5;

        stageClearCount = maxCount;

        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * 5}";
    }
    private void OnClickClearButton()
    {
        // TODO
        // 소탕
    }

    private void OnClickFormationButton()
    {
        Managers.UI.ShowUI<FormationUI>();

        // CommonUI의 경우 SortOrder = 0 고정
        // 다른 UI는 -20부터 1씩 증가하며 생성
        // StageInfoUI의 경우 CommonUI보다 위에 있어야 하기 때문에 10으로 고정
        // 이 때 편성UI로 넘어가면 SatgeInfoUI가 편성UI보다 위에 있게 됨
        // CloseUI는 UI의 가장 위를 닫기 때문에 편성UI가 생성된 시점에서 Close 불가
        // Close를 먼저 할 경우 평성 UI 생성 불가
        // 따라서 편성 UI 생성 후 StageInfoUI SetActive = false 형태로 구현
        // 편성UI의 BackButtons에서 StageInfoUI가 있다면 SetActive = true
        gameObject.SetActive(false);
    }

    private void OnClickEnterButton()
    {
        // TODO
        // 해당 스테이지 입장
        BattleLoadingController.LoadBattle("BattleScene");
    }
    private void OnClickEnemyInfoButton()
    {
        // TODO
        // 적 정보UI 생성
    }

    private void OnClickBackButton()
    {
        Managers.UI.CloseUI(this);
    }
}