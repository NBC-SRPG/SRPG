using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Constants;

public class MissionManager
{
    public event Action<int> OnMissionStartCallback;
    public event Action<int, int> OnMissionUpdateCallback; // TODO : 업데이트 콜백에서 미션 저장
    public event Action<int> OnMissionCompleteCallback;
    public event Action<int> OnMissionReceiveCallback;
    public MissionDB missionDB { get; set; }

    // 구독중인 미션들
    private Dictionary<MissionType, List<MissionSO>> subscribeMissions = new();

    // 마지막 일일 미션 초기화 날짜
    private DateTime lastDailyReset;
    // 마지막 주간 미션 초기화 날짜
    private DateTime lastWeeklyReset;

    public void Init()
    {
        missionDB = new();
    }

    public void SubscribeMission(int missionId)
    {
        Debug.Log("SubscribeMission " + missionId);

        var missionData = missionDB.Get(missionId);

        if (subscribeMissions.ContainsKey(missionData.missionType) == false)
        {
            subscribeMissions[missionData.missionType] = new List<MissionSO>();
        }

        subscribeMissions[missionData.missionType].Add(missionData);
    }

    public void UnsubscribeMission(int missionId)
    {
        Debug.Log("UnsubscribeQuest " + missionId);

        var missionData = missionDB.Get(missionId);

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

        if (Managers.AccountData.ongoingMissions.ContainsKey(missionId))
        {
            return;
        }

        Managers.AccountData.ongoingMissions.Add(missionId, mission);

        SubscribeMission(missionId);

        OnMissionStartCallback?.Invoke(missionId);

        SaveOngoingMission(missionId);
    }

    public void MissionUpdate(int missionId, int amount)
    {
        if (Managers.AccountData.ongoingMissions.ContainsKey(missionId) == false)
        {
            return;
        }

        var missiontData = missionDB.Get(missionId);

        int currentCount = Managers.AccountData.ongoingMissions[missionId].Update(amount);

        OnMissionUpdateCallback?.Invoke(missionId, amount);

        if (currentCount >= missiontData.count)
        {
            MissionClear(missionId);
        }

        SaveOngoingMission(missionId);
    }


    public void MissionClear(int missionId)
    {
        if (Managers.AccountData.ongoingMissions.ContainsKey(missionId) == false)
        {
            return;
        }

        Managers.AccountData.ongoingMissions[missionId].Complete();
        Managers.AccountData.ongoingMissions.Remove(missionId);

        Managers.AccountData.completeMissions.Add(missionId);

        OnMissionCompleteCallback?.Invoke(missionId);

        SaveOngoingMission(missionId);
        SaveCompleteMission(missionId);
    }

    public void MissionReceive(int missionId)
    {
        if (Managers.AccountData.completeMissions.Contains(missionId) == false)
        {
            return;
        }

        Managers.AccountData.completeMissions.Remove(missionId);
        Managers.AccountData.receiveMissions.Add(missionId);

        OnMissionReceiveCallback?.Invoke(missionId);

        SaveReceiveMission(missionId);
    }

    public bool IsClear(int id)
    {
        return Managers.AccountData.completeMissions.Contains(id);
    }


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
            foreach (var mission in Managers.AccountData.completeMissions)
            {
                if (missionDB.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    completeMissionsList.Add(mission);
                }
            }

            foreach (var mission in completeMissionsList)
            {
                // 완료 미션 중 일일 미션인 경우
                if (missionDB.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    Managers.AccountData.completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            List<int> receiveMissionsList = new();
            foreach (var mission in Managers.AccountData.receiveMissions)
            {
                if (missionDB.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    receiveMissionsList.Add(mission);
                }
            }
            foreach (var mission in receiveMissionsList)
            {
                // 수령한 미션 중 일일 미션인 경우
                if (missionDB.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    Managers.AccountData.receiveMissions.Remove(mission);

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

    // 월요일 0시 이후 첫 접속 시 주간 미션 초기화 해주기
    public void WeeklyMissionInit()
    {
        // 현재 시간을 가져오기
        DateTime now = DateTime.Now;

        // 현재 요일이 월요일이고, 마지막 초기화한 주가 현재 주와 다른 경우
        if (now.DayOfWeek == DayOfWeek.Monday && GetWeekOfYear(now) != GetWeekOfYear(lastWeeklyReset))
        {
            // 주간 미션 초기화 로직
            List<int> completeMissionsList = new();
            foreach (var mission in Managers.AccountData.completeMissions)
            {
                if (missionDB.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    completeMissionsList.Add(mission);
                }
            }

            foreach (var mission in completeMissionsList)
            {
                // 완료 미션 중 주간 미션인 경우
                if (missionDB.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    Managers.AccountData.completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            List<int> receiveMissionsList = new();
            foreach (var mission in Managers.AccountData.receiveMissions)
            {
                if (missionDB.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    receiveMissionsList.Add(mission);
                }
            }
            foreach (var mission in receiveMissionsList)
            {
                // 수령한 미션 중 주간 미션인 경우
                if (missionDB.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    Managers.AccountData.receiveMissions.Remove(mission);

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

    private void SaveOngoingMission(int missionId)
    {
        if (Managers.AccountData.ongoingMissions.TryGetValue(missionId, out Mission mission))
        {
            Managers.DB.Write<int>(Managers.DB.userDB.Child("missionData/ongoingMissionsData").Child(missionId.ToString()), mission.MissionProgress);
        }
        else
        {
            Managers.DB.Delete(Managers.DB.userDB.Child("missionData/ongoingMissionsData").Child(missionId.ToString()));
        }
    }

    private void SaveCompleteMission(int missionId)
    {
        Managers.DB.Write<bool>(Managers.DB.userDB.Child("missionData/completeMissionsData").Child(missionId.ToString()), false);
    }

    private void SaveReceiveMission(int missionId)
    {
        Managers.DB.Write<bool>(Managers.DB.userDB.Child("missionData/completeMissionsData").Child(missionId.ToString()), true);
    }
}