using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionData : MonoBehaviour //현재 missionTrigger, nextMissions, missionConditions는 임시로 작성된 상태입니다. 수정 및 기능 구현 / 추가 보완 작업이 필요합니다.
{
    public Dictionary<MissionTrigger, int> missionTrigger { get; private set; } //미션 트리거. 트리거 타입 / 트리거 조건 값
    public bool isActivate { get; private set; }
    public string missionID { get; private set; }  //퀘스트 ID
    public string missionName { get; private set; }  //UI에 표시될 퀘스트 이름 
    public string missionDescription { get; private set; }  //UI에 표시될 퀘스트 설명
    public bool isAchievement { get; set; }  //달성 여부. true → 달성 완료상태. false → 미달성 상태.
    public MissionCategory missionCategory { get; private set; }  //퀘스트 분류. 일일 / 주간 / 업적 / 초보자
    public Dictionary<object, int> missionRewards { get; private set; } //퀘스트 보상. object → 보상이 무엇인지. Int, string, enum, ItemData 등이 올 수 있음. int→ 보상의 양이 얼마인지.
    public MissionData[] nextMissions { get; private set; } //이 퀘스트가 클리어 된다면 UnRock 되는 다음 퀘스트 목록
    public struct missionConditions<T>
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

    // 3. 매개변수 A가 B 이하인지.
    public bool IsBelow<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) == -1;
    }

    public enum MissionTrigger
    {
        FirstLogin,
        TimeIn,
        TriggerOn
    }
    public enum MissionCategory
    {
        Daily,
        Weekly,
        Achievements,
        Newbie
    }


    // 생성자 메서드
    // 미션 조건 저장하는 방법 가이드 : T a에 매개변수 1, T b에 매개변수 1과 비교할 매개변수 또는 상수값, T c에 비교식 유형을 넣는다.

    // 사용 예시 (아무 몬스터나 5회 처지 조건을 저장하고 싶은 경우 )
    // a에 적 몬스터를 처치한 횟수를 담는 변수를 참조하게 한다. (이벤트 / 콜백을 통해 연결). b에 상수 5를 저장한다. 값이 같은지 체크하므로 c에 1을 입력한다. 
    public MissionData CreateMission<T>(
                                        Dictionary<MissionTrigger, int> missionTrigger,
                                      string missionID, string missionName, string missionDescription,
                                      MissionCategory missionCategory,
                                      Dictionary<object, int> missionRewards, T a, T b, int c,
                                         MissionData[] nextMissions
                                        )
    {
        MissionData missionData = new MissionData();
        missionData.missionTrigger = missionTrigger;
        missionData.missionID = missionID;
        missionData.missionName = missionName;
        missionData.missionDescription = missionDescription;
        missionData.missionCategory = missionCategory;
        missionData.missionRewards = missionRewards;
        missionData.nextMissions = nextMissions;

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
