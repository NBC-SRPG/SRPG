using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using static Constants;

public class MissionManager
{
    // 진행중인 미션들
    private Dictionary<int, Mission> ongoingMissions = new();
    // 완료 한 미션들
    private HashSet<int> completeMissions = new();
    // 보상 수령한 미션들
    private HashSet<int> receiveMissions = new();

    public IReadOnlyDictionary<int, Mission> OngoingMissions => ongoingMissions;
    public IReadOnlyCollection<int> CompleteMissions => completeMissions;
    public IReadOnlyCollection<int> ReceiveMissions => receiveMissions;

    public event Action<int> OnMissionStartCallback;
    public event Action<int, int> OnMissionUpdateCallback; // TODO : 업데이트 콜백에서 미션 저장
    public event Action<int> OnMissionCompleteCallback;
    public event Action<int> OnMissionReceiveCallback;

    // 구독중인 미션들
    private Dictionary<MissionType, List<MissionData>> subscribeMissions = new();

    // 마지막 일일 미션 초기화 날짜
    private DateTime lastDailyReset;
    // 마지막 주간 미션 초기화 날짜
    private DateTime lastWeeklyReset;

    private bool isInit = false;
    private bool isOngoingMissionsLoaded = false;
    private bool isCompleteMissionsLoaded = false;
    private bool hasOngoingMissions = false;
    private bool hasCompleteMissions = false;

    public void Init()
    {
        if (isInit == true)
        {
            return;
        }

        isInit = true;

        Managers.DB.Read(Managers.DB.userDB.Child("ongoingMissionsData"), OngoingMissionInit);
        Managers.DB.Read(Managers.DB.userDB.Child("completeMissionsData"), CompleteMissionInit);
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
                MainThreadExecutor.ExecuteInMainThread(() => MissionStart(int.Parse(mission.Key), Convert.ToInt32(mission.Value)));
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
                    MissionStart(int.Parse(mission.Key), TestDatabase.Mission.Get(int.Parse(mission.Key)).count);
                    // 바로 클리어 처리
                    MissionClear(int.Parse(mission.Key));

                    // 데이터의 값이 true라면 보상 수령을 한 것
                    if ((bool)mission.Value)
                    {
                        // 보상 수령 처리
                        MissionReceive(int.Parse(mission.Key));
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
                    // 여기에 기본 미션들을 설정
                    MissionStart(90001000);
                    MissionStart(90001001);
                    MissionStart(90001002);
                });
            }
        }
    }

    public void SubscribeMission(int missionId)
    {
        Debug.Log("SubscribeMission " + missionId);

        var missionData = TestDatabase.Mission.Get(missionId);

        if (subscribeMissions.ContainsKey(missionData.missionType) == false)
        {
            subscribeMissions[missionData.missionType] = new List<MissionData>();
        }

        subscribeMissions[missionData.missionType].Add(missionData);
    }

    public void UnsubscribeMission(int missionId)
    {
        Debug.Log("UnsubscribeQuest " + missionId);

        var missionData = TestDatabase.Mission.Get(missionId);

        if (subscribeMissions.ContainsKey(missionData.missionType) == false)
        {
            return;
        }

        subscribeMissions[missionData.missionType].Remove(missionData);
    }

    public void NotifyMission(MissionType type, int target, int count)
    {
        if (subscribeMissions.ContainsKey(type) == false)
        {
            return;
        }
            
        var filteredMissions = subscribeMissions[type];
        var targetMissions = filteredMissions.FindAll(q => q.target == target);
        foreach (var mission in targetMissions)
        {
            MissionUpdate(mission.missionId, count);
        }
    }

    // 미션 시작
    public void MissionStart(int missionId, int missionProgress = 0)
    {
        Debug.Log($"MissionStart: {missionId}");
        if (IsClear(missionId))
        {
            return;
        }

        Mission mission;

        if (missionProgress == 0)
        {
            mission = new Mission(missionId); // 메인 쓰레드에서만 객체 생성 가능하므로 비동기 콜백 불가능 MainThreadExecutor사용
            mission.Start();
        }
        else
        {
            mission = new Mission(missionId, missionProgress);
        }

        if (ongoingMissions.ContainsKey(missionId))
        {
            return;
        }

        ongoingMissions.Add(missionId, mission);

        SubscribeMission(missionId);

        OnMissionStartCallback?.Invoke(missionId);
    }

    public void MissionUpdate(int missionId, int amount)
    {
        if (ongoingMissions.ContainsKey(missionId) == false)
        {
            return;
        }

        var missiontData = TestDatabase.Mission.Get(missionId);

        int currentCount = ongoingMissions[missionId].Update(amount);

        OnMissionUpdateCallback?.Invoke(missionId, amount);

        if (currentCount >= missiontData.count)
        {
            MissionClear(missionId);
        }
    }


    public void MissionClear(int missionId)
    {
        if (ongoingMissions.ContainsKey(missionId) == false)
        {
            return;
        }

        ongoingMissions[missionId].Complete();
        ongoingMissions.Remove(missionId);

        completeMissions.Add(missionId);

        OnMissionCompleteCallback?.Invoke(missionId);
    }

    public void MissionReceive(int missionId)
    {
        if (completeMissions.Contains(missionId) == false)
        {
            return;
        }

        completeMissions.Remove(missionId);
        receiveMissions.Add(missionId);

        OnMissionReceiveCallback?.Invoke(missionId);
    }

    public bool IsClear(int id)
    {
        return completeMissions.Contains(id);
    }

    // TODO
    // 0시 이후 첫 접속 시 일일 미션 초기화 해주기
    public void DailyMissionInit()
    {
        Debug.Log($"DailyMissionInit: last - {lastDailyReset.Date}");
        // 현재 시간 가져오기
        DateTime now = DateTime.Now;

        // TODO
        // DB에서 마지막 초기화 시간 가져와서 비교하기
        if (now.Date > lastDailyReset.Date)
        {
            // 일일 미션 초기화 로직
            List<int> completeMissionsList = new();
            foreach (var mission in completeMissions)
            {
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    completeMissionsList.Add(mission);
                }
            }

            foreach (var mission in completeMissionsList)
            {
                // 완료 미션 중 일일 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            List<int> receiveMissionsList = new();
            foreach (var mission in receiveMissions)
            {
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    receiveMissionsList.Add(mission);
                }
            }
            foreach (var mission in receiveMissionsList)
            {
                // 수령한 미션 중 일일 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    receiveMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            // 마지막 초기화 시간 업데이트
            lastDailyReset = now;
            // TODO
            // DB에 마지막 초기화 시간 저장

            // 미션 시작 이벤트
            OnMissionStartCallback?.Invoke(0);
        }
    }

    // TODO
    // 월요일 0시 이후 첫 접속 시 주간 미션 초기화 해주기
    private void WeeklyMissionInit()
    {
        // 현재 시간을 가져오기
        DateTime now = DateTime.Now;

        // 현재 요일이 월요일이고, 마지막 초기화한 주가 현재 주와 다른 경우
        if (now.DayOfWeek == DayOfWeek.Monday && GetWeekOfYear(now) != GetWeekOfYear(lastWeeklyReset))
        {
            // 주간 미션 초기화 로직
            List<int> completeMissionsList = new();
            foreach (var mission in completeMissions)
            {
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    completeMissionsList.Add(mission);
                }
            }

            foreach (var mission in completeMissionsList)
            {
                // 완료 미션 중 주간 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            List<int> receiveMissionsList = new();
            foreach (var mission in receiveMissions)
            {
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    receiveMissionsList.Add(mission);
                }
            }
            foreach (var mission in receiveMissionsList)
            {
                // 수령한 미션 중 주간 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    receiveMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            // 마지막 초기화 날짜 업데이트
            lastWeeklyReset = now;
            // TODO
            // DB에 마지막 초기화 시간 저장
        }
    }

    // 현재 날짜가 속한 주의 번호를 반환하는 메서드
    private int GetWeekOfYear(DateTime date)
    {
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        // date가 포함된 주 가져오기
        int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum;
    }
}