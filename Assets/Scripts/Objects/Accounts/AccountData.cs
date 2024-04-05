using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
 
    //public Dictionary<int, int> inventory { get; set; }  Todo: 아이템 데이터 추가 시 활성화 필요
    public Dictionary<int, string[]> friendData { get; set; }
    public Dictionary<int, FormationData> formationData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }
    public List<MailSO> mailBox { get; set; }

    public Dictionary<int, Mission> ongoingMissions { get; set; } = new(); // 진행중인 미션들

    public HashSet<int> completeMissions { get; set; } = new(); // 완료 한 미션들

    public HashSet<int> receiveMissions { get; set; } = new();// 보상 수령한 미션들

    //public IReadOnlyDictionary<int, Mission> OngoingMissions => ongoingMissions;
    //public IReadOnlyCollection<int> CompleteMissions => completeMissions;
    //public IReadOnlyCollection<int> ReceiveMissions => receiveMissions;

    private bool isInit = false;
    private bool isOngoingMissionsLoaded = false;
    private bool isCompleteMissionsLoaded = false;
    private bool hasOngoingMissions = false;
    private bool hasCompleteMissions = false;

    /*
    public void Init()
    {
        
    }
    */

    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        //List<int> ongoingMissions,
        //List<int> completeMissions,
        //List<int> receiveMissions,
        //Dictionary<int, int> inventory,
        Dictionary<int, string[]> friendData,
        Dictionary<int, FormationData> formationData,
        List<MailSO> mailBox
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        //this.inventory = inventory ?? new Dictionary<int, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
        this.formationData = formationData ?? new Dictionary<int, FormationData>();
        this.mailBox = mailBox ?? new List<MailSO>();

        if (isInit == true)
        {
            return;
        }

        isInit = true;

        Managers.DB.Read(Managers.DB.userDB.Child("missionData").Child("ongoingMissionsData"), OngoingMissionInit);
        Managers.DB.Read(Managers.DB.userDB.Child("missionData").Child("completeMissionsData"), CompleteMissionInit);
    }

    private void OngoingMissionInit(DataSnapshot snapshot)
    {
        // 데이터가 존재하는지 확인
        if (snapshot.Exists && snapshot.ChildrenCount > 0)
        {
            // 데이터가 있다면 순회
            foreach (DataSnapshot mission in snapshot.Children)
            {
                // 데이터의 키(미션Id)와 값(진행정도)으로 MissionStart를 메인쓰레드에서 실행 
                MainThreadExecutor.ExecuteInMainThread(() => Managers.Mission.MissionStart(int.Parse(mission.Key), Convert.ToInt32(mission.Value)));
                Debug.Log(mission.Key + ":" + mission.Value);
            }
            // 데이터가 있음을 체크
            hasOngoingMissions = true;
        }
        else
        {
            // 데이터가 없으면 hasOngoingMissions은 false
            hasOngoingMissions = false;
        }
        // 진행 중 미션 초기화 완료
        isOngoingMissionsLoaded = true;

        CheckAndInitializeDefaultMissions();
    }

    private void CompleteMissionInit(DataSnapshot snapshot)
    {
        // 데이터가 존재하는지 확인
        if (snapshot.Exists && snapshot.ChildrenCount > 0)
        {
            // 데이터가 있다면 순회
            foreach (DataSnapshot mission in snapshot.Children)
            {
                // 데이터의 키(미션Id)로 미션 시작, 진행 정도는 완료 미션이기 때문에 해당 미션의 count를 그대로 적용
                MainThreadExecutor.ExecuteInMainThread(() => {
                    Managers.Mission.MissionStart(int.Parse(mission.Key), TestDatabase.Mission.Get(int.Parse(mission.Key)).count);
                    // 바로 클리어 처리
                    Managers.Mission.MissionClear(int.Parse(mission.Key));

                    // 데이터의 값이 true라면 보상 수령을 한 것
                    if ((bool)mission.Value)
                    {
                        // 보상 수령 처리
                        Managers.Mission.MissionReceive(int.Parse(mission.Key));
                    }
                });
                Debug.Log(mission.Key + ":" + mission.Value);
            }
            // 데이터가 있음을 체크
            hasCompleteMissions = true;
        }
        else
        {
            // 데이터가 없으면 hasCompleteMissions은 false
            hasCompleteMissions = false;
        }
        // 완료 미션 초기화 완료
        isCompleteMissionsLoaded = true;

        CheckAndInitializeDefaultMissions();
    }

    private void CheckAndInitializeDefaultMissions()
    {
        // 두 데이터 로드가 모두 완료되었는지 확인
        if (isOngoingMissionsLoaded && isCompleteMissionsLoaded)
        {
            // 두 데이터가 모두 비어 있으면 기본 미션 설정
            if (!hasOngoingMissions && !hasCompleteMissions)
            {
                MainThreadExecutor.ExecuteInMainThread(() =>
                {
                    // 기본 미션들을 설정
                    Managers.Mission.MissionStart(90001000);
                    Managers.Mission.MissionStart(90001001);
                    Managers.Mission.MissionStart(90001002);
                });
            }
        }
    }
}
