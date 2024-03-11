using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionData : MonoBehaviour
{
    protected int missionID;
    protected string missionName;
    protected string missionDescription;
    protected bool isAchievement;
    protected _MissionType missionType;
    protected Dictionary<object, int> missionRewards;
    protected struct missionConditions<T>
    {
        public T Number1 { get; set; } //매개변수 1
        public T Number2 { get; set; } //매개변수 2 또는 상수값
        public int Number3 { get; set; } //조건문 형식
    }


    // 1. 매개변수 A와 B가 같은지.
    public bool IsEqual<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) == 0;
    }

    // 2. 매개변수 A가 B 초과인지.
    public bool IsOver<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) == 1;
    }

    public enum _MissionType
    {
        Daily,
        Weekly,
        Achievements,
        Newbie
    }

    // 생성자 메서드
    // 미션 조건 저장하는 방법 가이드 : T a에 매개변수 1, T b에 매개변수 1과 비교할 매개변수 또는 상수값, T c에 비교식 유형을 넣는다.
    // 사용 예시 (아무 몬스터나 5회 처지 조건을 저장하고 싶은 경우 ) : a에 적 몬스터를 kill한 횟수 값을 가져다 넣는다(이벤트나 콜백을 통해 연결시켜 놓기). b에 상수 5를 입력한다. c에 1을 입력한다. 
    protected MissionData CreateMission<T>(int missionID, string missionName, string missionDescription,
                                      _MissionType missionType,
                                      Dictionary<object, int> missionRewards, T a, T b, int c)
    {
        MissionData missionData = new MissionData();
        missionData.missionID = missionID;
        missionData.missionName = missionName;
        missionData.missionDescription = missionDescription;
        missionData.missionType = missionType;
        missionData.missionRewards = missionRewards;

        // missionConditions 구조체 초기화
        missionConditions<T> missionConditions = new missionConditions<T>
        {
            Number1 = a,
            Number2 = b,
            Number3 = c
        };
        return missionData;
    }

    void Start()
    {
    }
}
