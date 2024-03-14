using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionData : MonoBehaviour //���� missionTrigger, nextMissions, missionConditions�� �ӽ÷� �ۼ��� �����Դϴ�. ���� �� ��� ���� / �߰� ���� �۾��� �ʿ��մϴ�.
{
    public Dictionary<MissionTrigger, int> missionTrigger { get; private set; } //�̼� Ʈ����. Ʈ���� Ÿ�� / Ʈ���� ���� ��
    public bool isActivate { get; private set; }
    public string missionID { get; private set; }  //����Ʈ ID
    public string missionName { get; private set; }  //UI�� ǥ�õ� ����Ʈ �̸� 
    public string missionDescription { get; private set; }  //UI�� ǥ�õ� ����Ʈ ����
    public bool isAchievement { get; set; }  //�޼� ����. true �� �޼� �Ϸ����. false �� �̴޼� ����.
    public MissionCategory missionCategory { get; private set; }  //����Ʈ �з�. ���� / �ְ� / ���� / �ʺ���
    public Dictionary<object, int> missionRewards { get; private set; } //����Ʈ ����. object �� ������ ��������. Int, string, enum, ItemData ���� �� �� ����. int�� ������ ���� ������.
    public MissionData[] nextMissions { get; private set; } //�� ����Ʈ�� Ŭ���� �ȴٸ� UnRock �Ǵ� ���� ����Ʈ ���
    public struct missionConditions<T>
    {
        public T Number1 { get; set; } //�Ű����� 1
        public T Number2 { get; set; } //�Ű����� 2 �Ǵ� �����
        public int Number3 { get; set; } //���ǹ� ����
    }

    // 1. �Ű����� A�� B�� ������.
    public bool IsEqual<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) == 0;
    }

    // 2. �Ű����� A�� B �ʰ�����.
    public bool IsOver<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) == 1;
    }

    // 3. �Ű����� A�� B ��������.
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


    // ������ �޼���
    // �̼� ���� �����ϴ� ��� ���̵� : T a�� �Ű����� 1, T b�� �Ű����� 1�� ���� �Ű����� �Ǵ� �����, T c�� �񱳽� ������ �ִ´�.

    // ��� ���� (�ƹ� ���ͳ� 5ȸ ó�� ������ �����ϰ� ���� ��� )
    // a�� �� ���͸� óġ�� Ƚ���� ��� ������ �����ϰ� �Ѵ�. (�̺�Ʈ / �ݹ��� ���� ����). b�� ��� 5�� �����Ѵ�. ���� ������ üũ�ϹǷ� c�� 1�� �Է��Ѵ�. 
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

        // missionConditions ����ü �ʱ�ȭ
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
