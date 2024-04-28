using System;
using UnityEngine;
using static Constants;
public class StageInfoUI : UIBase
{
    private bool isInit = false;
    private int stageClearCount = 0;
    private StageSO stage;

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

    private enum GameObjects
    {
        RewardContent
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

        this.stage = stage;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetText((int)Texts.StageNumText).text = stage.stageNumber;
        GetText((int)Texts.StageNameText).text = $"{stage.stageName}";
        GetText((int)Texts.StageLevelText).text = $"권장레벨 {stage.recommendLevel}";

        // TODO
        // 목표를 달성 했는지 어떻게 알지??
        if (stage.GetExtraGoalDetail(0) != "")
        {
            GetText((int)Texts.Goal).text += "\n☆";
            GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(0);
        }
        if (stage.GetExtraGoalDetail(1) != "")
        {
            GetText((int)Texts.Goal).text += "\n☆";
            GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(1);
        }
        if (stage.GetExtraGoalDetail(2) != "")
        {
            GetText((int)Texts.Goal).text += "\n☆";
            GetText((int)Texts.Goal).text += stage.GetExtraGoalDetail(2);
        }

        GetButton((int)Buttons.FormationButton).onClick.AddListener(OnClickFormationButton);
        GetButton((int)Buttons.ClearDecreButton).onClick.AddListener(OnClickClearDecreButton);
        GetButton((int)Buttons.ClearIncreButton).onClick.AddListener(OnClickClearIncreButton);
        GetButton((int)Buttons.ClearMaxButton).onClick.AddListener(OnClickClearMaxButton);
        GetButton((int)Buttons.ClearButton).onClick.AddListener(OnClickClearButton);
        GetButton((int)Buttons.EnterButton).onClick.AddListener(OnClickEnterButton);
        GetButton((int)Buttons.EnemyInfoButton).onClick.AddListener(OnClickEnemyInfoButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);

        InitImage();
        InitReward();

        isInit = true;

        Managers.GameManager.thisStage = stage;
    }

    private void InitImage()
    {
        for (int i = 0; i < 5; i++)
        {
            Images partyImageEnum = (Images)Enum.Parse(typeof(Images), $"Party{i + 1}Image");
            int characterId = Managers.AccountData.formationData[presetIndex].characterId[i];

            if (characterId == 0)
            {
                GetImage((int)partyImageEnum).sprite = null;
                continue;
            }

            Sprite icon = Managers.AccountData.characterData[characterId].SO.icon;
            GetImage((int)partyImageEnum).sprite = icon;
        }
    }

    private void InitReward()
    {
        if (stage.exp > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(stage.exp.ToString(), "Exp");
        }

        if (stage.gold > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(stage.gold.ToString(), "Gold");
        }

        foreach (var reward in stage.rewards)
        {
            Utility.Id2SO<ItemSO>(reward.Key, (item) =>
            {
                GameObject go = Managers.Resource.Instantiate(
                    Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                    GetObject((int)GameObjects.RewardContent).transform);

                go.GetComponent<RewardIconUI>().Init(reward.Value.ToString(), (item as ItemSO).icon);
            });
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
        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * ConsumeAp}";
    }
    private void OnClickClearIncreButton()
    {
        if ((stageClearCount + 1) * 5 > Managers.AccountData.playerData.Ap)
        {
            return;
        }

        stageClearCount++;

        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * ConsumeAp}";
    }

    private void OnClickClearMaxButton()
    {
        int maxCount = Managers.AccountData.playerData.Ap / ConsumeAp;

        stageClearCount = maxCount;

        GetText((int)Texts.ClearCountText).text = $"{stageClearCount * ConsumeAp}";
    }
    private void OnClickClearButton()
    {
        if (stageClearCount == 0)
        {
            return;
        }

        if (Managers.AccountData.stageClearData.TryGetValue(stage.stageNumber, out int starNum))
        {
            if (starNum < 3)
            {
                Managers.UI.ShowUI<WarningUI>().Init("스테이지 별 3개로 클리어 후 소탕이 가능합니다.");

                return;
            }
        }
        else
        {
            Managers.UI.ShowUI<WarningUI>().Init("스테이지 별 3개로 클리어 후 소탕이 가능합니다.");
            
            return;
        }

        // AP 감소
        Managers.AccountData.playerData.ReduceAP(stageClearCount * ConsumeAp);
        // 보상 획득
        if (stage.exp > 0)
        {
            Managers.AccountData.playerData.AddExp(stage.exp);
        }

        if (stage.gold > 0)
        {
            Managers.AccountData.playerData.AddExp(stage.gold);
        }

        foreach (var reward in stage.rewards)
        {
            Managers.AccountData.AcquireItems(reward.Key, reward.Value);
        }

        Managers.UI.ShowUI<WarningUI>().Init("소탕 완료");
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
        // 해당 스테이지 입장
        Managers.GameManager.UpdateParty(Managers.AccountData.formationData[Constants.presetIndex]);
        BattleLoadingController.LoadBattle("BattleScene");
    }
    private void OnClickEnemyInfoButton()
    {
        // 적 정보UI 생성
        Managers.UI.ShowUI<EnemyInfoUI>().Init(stage);
    }

    private void OnClickBackButton()
    {
        Managers.UI.CloseUI(this);
    }
}