using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class RewardGetUI : UIBase
{
    private int missionId;
    private enum Buttons
    {
        CheckButton
    }
    
    private enum GameObjects
    {
        Content
    }

    public void Init(int missionId)
    {
        this.missionId = missionId;

        MissionSO missionData = Managers.Mission.missionDB.Get(missionId);

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);

        // 경험치 보상 아이콘 생성
        CreateRewardIcon(missionData.exp, TestExpImage.ToString());
        // ap 보상 아이콘 생성
        CreateRewardIcon(missionData.ap, TestApImage.ToString());
        // 골드 보상 아이콘 생성
        CreateRewardIcon(missionData.gold, TestGoldImage.ToString());
        // 다이아 보상 아이콘 생성
        CreateRewardIcon(missionData.diamond, TestDiamondImage.ToString());
        // 아이템 리워드 아이콘 생성
        foreach (var itemReward in missionData.rewards)
        {
            CreateRewardIcon(itemReward.Key, itemReward.Value.ToString());
        }
    }

    // 모두 받기 Init
    public void InitWithMultipleRewards(int totalExp, int totalAp, int totalGold, int totalDiamond, Dictionary<int, int> itemRewards)
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CheckButton).onClick.AddListener(OnClickCheckButton);

        CreateRewardIcon(totalExp, TestExpImage.ToString());
        CreateRewardIcon(totalAp, TestApImage.ToString());
        CreateRewardIcon(totalGold, TestGoldImage.ToString());
        CreateRewardIcon(totalDiamond, TestDiamondImage.ToString());

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

    private void OnClickCheckButton()
    {
        Debug.Log("OnClickCheckButton");

        Managers.UI.CloseUI(this);
    }
}
