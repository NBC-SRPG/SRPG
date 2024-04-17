using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Constants;

public class MissionEntryUI : UIBase
{
    public int missionId;
    private MissionSO missionData;

    private enum Texts
    {
        MissionNameText,
        MissionDescriptionText,
        MissionProgressText,
        GetOrGoText
    }

    private enum Images
    {
        MissionFrontProgressImage,
        GetOrGoImage,
        ReceiveCheckImage,
        ReceiveBackImage
    }

    private enum Buttons
    {
        GetOrGoButton
    }

    private enum GameObjects
    {
        RewardContent
    }

    private void OnDestroy()
    {
        Managers.Mission.OnMissionUpdateCallback -= UpdateMissionUI;
        Managers.Mission.OnMissionCompleteCallback -= CompleteMissionUI;
        Managers.Mission.OnMissionReceiveCallback -= ReceiveMissionUI;
    }

    // 미션id와 완료 된 미션인지를 받음
    public void Init(int missionId)
    {
        this.missionId = missionId;

        // 미션 업데이트 콜백 구독
        Managers.Mission.OnMissionUpdateCallback += UpdateMissionUI;
        Managers.Mission.OnMissionCompleteCallback += CompleteMissionUI;
        Managers.Mission.OnMissionReceiveCallback += ReceiveMissionUI;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        missionData = TestDatabase.Mission.Get(missionId);

        GetText((int)Texts.MissionNameText).text = missionData.name;
        GetText((int)Texts.MissionDescriptionText).text = missionData.missionDescription;

        // 경험치 보상 아이콘 생성
        if (missionData.exp > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(missionData.exp.ToString(), TestExpImage);
        }
        // ap 보상 아이콘 생성
        if (missionData.ap > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(missionData.ap.ToString(), TestApImage);
        }
        // 골드 보상 아이콘 생성
        if (missionData.gold > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(missionData.gold.ToString(), TestGoldImage);
        }
        // 다이아 보상 아이콘 생성
        if (missionData.diamond > 0)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.RewardContent).transform);

            go.GetComponent<RewardIconUI>().Init(missionData.diamond.ToString(), TestDiamondImage);
        }
        // TODO
        // 아이템 보상 아이콘 생성

        // 수령 체크 이미지 끄기
        GetImage((int)Images.ReceiveCheckImage).gameObject.SetActive(false);

        // 수령 체크 배경 끄기
        GetImage((int)Images.ReceiveBackImage).gameObject.SetActive(false);

        // 진행 중인 미션
        if (Managers.AccountData.ongoingMissions.ContainsKey(missionId))
        {
            OngoingMissionInit();
        }
        // 완료 미션
        else if (Managers.AccountData.completeMissions.Contains(missionId))
        {
            CompleteMissionInit();
        }
        // 수령 미션
        else if (Managers.AccountData.receiveMissions.Contains(missionId))
        {
            ReceiveMissionInit();
        }
    }

    // TODO
    // 진행 중, 완료로 나누지 말고 보상 수령 했는지 안했는지로 나눠어야 할 것 같음

    // 완료 미션 초기화
    private void CompleteMissionInit()
    {
        GetText((int)Texts.MissionProgressText).text = $"{missionData.count} / {missionData.count}";
        GetImage((int)Images.MissionFrontProgressImage).fillAmount = 1;

        GetImage((int)Images.GetOrGoImage).color = Color.blue;
        GetText((int)Texts.GetOrGoText).text = "받기";
        GetButton((int)Buttons.GetOrGoButton).onClick.AddListener(OnClickGetButton);
    }

    // 진행 미션 초기화
    private void OngoingMissionInit()
    {
        Mission mission = Managers.AccountData.ongoingMissions[missionId];

        GetText((int)Texts.MissionProgressText).text = $"{mission.MissionProgress} / {missionData.count}";
        GetImage((int)Images.MissionFrontProgressImage).fillAmount = (float)mission.MissionProgress/ missionData.count;

        GetImage((int)Images.GetOrGoImage).color = Color.yellow;
        GetText((int)Texts.GetOrGoText).text = "바로가기";
        GetButton((int)Buttons.GetOrGoButton).onClick.AddListener(OnClickGoButton);
    }

    // 수령 미션 초기화
    private void ReceiveMissionInit()
    {
        GetText((int)Texts.MissionProgressText).text = $"{missionData.count} / {missionData.count}";
        GetImage((int)Images.MissionFrontProgressImage).fillAmount = 1;

        GetImage((int)Images.GetOrGoImage).color = Color.blue;
        GetText((int)Texts.GetOrGoText).text = "받기";

        // 수령 체크 이미지 켜기
        GetImage((int)Images.ReceiveCheckImage).gameObject.SetActive(true);

        // 수령 체크 배경 켜기
        GetImage((int)Images.ReceiveBackImage).gameObject.SetActive(true);
    }

    // 미션이 업데이트 되면 UI 업데이트 해주기
    private void UpdateMissionUI(int updateMissionId, int amount)
    {
        Debug.Log($"UpdateMissionUI: {updateMissionId}");

        // 업데이트 된 미션 id와 같은 id를 가졌을 때만 업데이트
        if (missionId == updateMissionId)
        {
            Mission mission = Managers.AccountData.ongoingMissions[updateMissionId];

            GetText((int)Texts.MissionProgressText).text = $"{mission.MissionProgress} / {missionData.count}";
            GetImage((int)Images.MissionFrontProgressImage).fillAmount = (float)mission.MissionProgress / missionData.count;
        }
    }

    // 미션이 완료되면 UI 업데이트 해주기
    private void CompleteMissionUI(int completeMissionId)
    {
        Debug.Log($"CompleteMissionUI: {completeMissionId}");

        // 완료 된 미션 id와 같은 id를 가졌을 때만 업데이트
        if (missionId == completeMissionId)
        {
            GetText((int)Texts.MissionProgressText).text = $"{missionData.count} / {missionData.count}";
            GetImage((int)Images.MissionFrontProgressImage).fillAmount = 1f;

            GetImage((int)Images.GetOrGoImage).color = Color.blue;
            GetText((int)Texts.GetOrGoText).text = "받기";

            GetButton((int)Buttons.GetOrGoButton).onClick.RemoveAllListeners();
            GetButton((int)Buttons.GetOrGoButton).onClick.AddListener(OnClickGetButton);
        }
    }

    private void ReceiveMissionUI(int receiveMissionId)
    {
        Debug.Log($"ReceiveMissionUI: {receiveMissionId}");

        // 완료 된 미션 id와 같은 id를 가졌을 때만 업데이트
        if (missionId == receiveMissionId)
        {
            // 수령 체크 이미지 켜키
            GetImage((int)Images.ReceiveCheckImage).gameObject.SetActive(true);

            // 수령 체크 배경 켜기
            GetImage((int)Images.ReceiveBackImage).gameObject.SetActive(true);
        }
    }

    private void NextMissionStart()
    {
        foreach (var nextMission in TestDatabase.Mission.Get(missionId).nextMissions)
        {
            Managers.Mission.MissionStart(nextMission);
        }
    }

    private void OnClickGetButton()
    {
        Debug.Log("OnClickGetButton");

        // 보상 획득 불가 (한도초과) 하다면 경고 UI 띄운 후 return
        if (CanClaimReward() == false)
        {
            return;
        }

        // 보상 수령
        ClaimReward();

        // 보상 획득 완료 UI 생성
        RewardGetUI ui = Managers.UI.ShowUI<RewardGetUI>();
        ui.Init(missionId);

        // 해당 미션 엔트리 수령 처리
        Managers.Mission.MissionReceive(missionId);

        // 다음 해금 미션 있다면 생성
        NextMissionStart();
    }

    private void ClaimReward()
    {
        // 경험치 보상
        if (missionData.exp > 0)
        {
            Managers.AccountData.playerData.AddExp(missionData.exp);
        }
        // ap 보상
        if (missionData.ap > 0)
        {
            Managers.AccountData.playerData.AddAP(missionData.ap);
        }
        // 골드 보상
        if (missionData.gold > 0)
        {
            Managers.AccountData.playerData.AddGold(missionData.gold);
        }
        // 다이아 보상
        if (missionData.diamond > 0)
        {
            Managers.AccountData.playerData.AddDiamond(missionData.diamond);
        }
        // 아이템 보상
    }

    // 보상을 받았을 때 한도를 넘어가는지 체크 
    private bool CanClaimReward()
    {
        Debug.Log("CanClaimReward");

        // 골드 획득 가능 체크
        if (Managers.AccountData.playerData.CanAddGold(missionData.gold) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.SetText(missionData.gold < 0 ? "골드 보상이 음수입니다." : "골드 보유 한도 초과");

            return false;
        }

        // 다이아 획득 가능 체크
        if (Managers.AccountData.playerData.CanAddDiamond(missionData.diamond) == false)
        {
            WarningUI ui = Managers.UI.ShowUI<WarningUI>();
            ui.SetText(missionData.diamond < 0 ? "다이아 보상이 음수입니다." : "다이아 보유 한도 초과");

            return false;
        }

        return true;
    }

    private void OnClickGoButton()
    {
        Debug.Log("OnClickGoButton");

        // TODO
        // 미션 타입에 따라 해당 UI로 넘어가게 하기
    }
}
