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
    public event Action<int, int> OnMissionUpdateCallback;
    public event Action<int> OnMissionCompleteCallback;
    public event Action<int> OnMissionReceiveCallback;

    // 구독중인 미션들
    private Dictionary<MissionType, List<MissionData>> subscribeMissions = new();

    // 마지막 일일 미션 초기화 날짜
    private DateTime lastDailyReset;
    // 마지막 주간 미션 초기화 날짜
    private DateTime lastWeeklyReset;

    public void Init()
    {
        // TODO
        // 처음 기본 미션 설정 필요
        // ongoingMissions, completeMission, receiveMissions DB에 저장 필요
        // 테스트 데이터
        MissionStart(90001000);
        MissionStart(90001001);
        //MissionStart(90001002);
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
            MissionUpdate(mission.id, count);
        }
    }

    // 미션 시작
    public void MissionStart(int missionId)
    {
        if (IsClear(missionId))
        {
            return;
        }

        var mission = new Mission(missionId);
        mission.Start();

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
            foreach (var mission in completeMissions)
            {
                // 완료 미션 중 일일 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            foreach (var mission in receiveMissions)
            {
                // 수령한 미션 중 일일 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Daily)
                {
                    completeMissions.Remove(mission);

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
            foreach (var mission in completeMissions)
            {
                // 완료 미션 중 주간 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    completeMissions.Remove(mission);

                    MissionStart(mission);
                }
            }

            foreach (var mission in receiveMissions)
            {
                // 수령한 미션 중 주간 미션인 경우
                if (TestDatabase.Mission.Get(mission).missionCategory == MissionCategory.Weekly)
                {
                    completeMissions.Remove(mission);

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