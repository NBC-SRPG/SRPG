using System.Collections.Generic;
using UnityEngine;

public class RewardGetUI : UIBase
{
    private enum Texts
    {
        TitleText
    }

    private enum Buttons
    {
        BackImage,
        CheckButton,
        CloseButton
    }
    
    private enum GameObjects
    {
        Content
    }

    public void Init(int missionId)
    {
        MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.BackImage).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickCheckButton);

        // 경험치 보상 아이콘 생성
        CreateRewardIcon(missionData.exp, "Exp");
        // ap 보상 아이콘 생성
        CreateRewardIcon(missionData.ap, "AP");
        // 골드 보상 아이콘 생성
        CreateRewardIcon(missionData.gold, "Gold");
        // 다이아 보상 아이콘 생성
        CreateRewardIcon(missionData.diamond, "Diamond");
        // 아이템 리워드 아이콘 생성
        foreach (var itemReward in missionData.rewards)
        {
            CreateRewardIcon(itemReward.Value, itemReward.Key.ToString());
        }
    }

    public void Init(StageSO stage, int clearCount)
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetText((int)Texts.TitleText).text = "소탕 완료";
        GetButton((int)Buttons.BackImage).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickCheckButton);

        // 경험치 보상 아이콘 생성
        CreateRewardIcon(stage.exp * clearCount, "Exp");
        // 골드 보상 아이콘 생성
        CreateRewardIcon(stage.gold * clearCount, "Gold");
        // 아이템 리워드 아이콘 생성
        foreach (var itemReward in stage.rewards)
        {
            Utility.Id2SO<ItemSO>(itemReward.Key, (result) =>
            {
                CreateRewardIcon(itemReward.Value * clearCount, (result as ItemSO).icon);
            });
        }
    }

    // 모두 받기 Init
    public void InitWithMultipleRewards(int totalExp, int totalAp, int totalGold, int totalDiamond, Dictionary<int, int> itemRewards)
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.BackImage).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickCheckButton);

        CreateRewardIcon(totalExp, "Exp");
        CreateRewardIcon(totalAp, "AP");
        CreateRewardIcon(totalGold, "Gold");
        CreateRewardIcon(totalDiamond, "Daimond");

        foreach (var itemReward in itemRewards)
        {
            CreateRewardIcon(itemReward.Value, itemReward.Key.ToString());
        }
    }
    private void CreateRewardIcon(int reward, string rewardId)
    {
        if (reward <= 0)
        {
            return;
        }

        GameObject go = Managers.Resource.Instantiate(
            Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
            GetObject((int)GameObjects.Content).transform);

        go.GetComponent<RewardIconUI>().Init(reward.ToString(), rewardId);
    }

    private void CreateRewardIcon(int reward, Sprite rewardSprite)
    {
        if (reward <= 0)
        {
            return;
        }

        GameObject go = Managers.Resource.Instantiate(
            Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
            GetObject((int)GameObjects.Content).transform);

        go.GetComponent<RewardIconUI>().Init(reward.ToString(), rewardSprite);
    }

    private void OnClickCheckButton()
    {
        Debug.Log("OnClickCheckButton");

        Managers.UI.CloseUI(this);
    }
}
