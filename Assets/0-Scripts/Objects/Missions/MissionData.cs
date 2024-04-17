using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu(menuName = "MissionData", fileName = "MissionSO_")]
public class MissionSO : ScriptableObject
{
    public int missionId; // 미션 id
    public string missionName; // 미션 이름
    public string missionDescription; // 미션 설명
    public MissionType missionType; // 미션 타입 (몬스터 처치, 스테이지 클리어 ...)
    public MissionCategory missionCategory; // 미션 분류 (일일, 주간 ...)
    public int target; // 미션 타겟 id (몬스터 id, 스테이지 id ...)
    public int count; // 미션 타겟 카운트 (몬스터 n마리, 스테이지 별 n개 이상 ...)
    public int exp; // 미션 보상 겸험치
    public int ap; // 미션 보상 ap
    public int gold; // 미션 보상 골드
    public int diamond; // 미션 보상 다이아
    public Dictionary<int, int> rewards = new(); // 미션 보상 아이템
    public List<int> nextMissions = new(); // 클리어 시 다음 해금 미션 id
}