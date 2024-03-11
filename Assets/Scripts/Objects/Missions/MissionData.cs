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

    public enum _MissionType
    {
        Daily,
        Weekly,
        Achievements,
        Newbie
    }

    // ������ �޼���
    // �̼� ���� �����ϴ� ��� ���̵� : T a�� �Ű����� 1, T b�� �Ű����� 1�� ���� �Ű����� �Ǵ� �����, T c�� �񱳽� ������ �ִ´�.
    // ��� ���� (�ƹ� ���ͳ� 5ȸ ó�� ������ �����ϰ� ���� ��� ) : a�� �� ���͸� kill�� Ƚ�� ���� ������ �ִ´�(�̺�Ʈ�� �ݹ��� ���� ������� ����). b�� ��� 5�� �Է��Ѵ�. c�� 1�� �Է��Ѵ�. 
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
