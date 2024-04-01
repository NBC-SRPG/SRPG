using System.Collections.Generic;
using static Constants;
public class MissionData
{
    public int id { get; set; } // 미션 id
    public string name { get; set; } // 미션 이름
    public string description { get; set; } // 미션 설명
    public MissionType missionType { get; set; } // 미션 타입 (몬스터 처치, 스테이지 클리어 ...)
    public MissionCategory missionCategory { get; set; } // 미션 분류 (일일, 주간 ...)
    public int target { get; set; } // 미션 타겟 id (몬스터 id, 스테이지 id ...)
    public int count { get; set; } // 미션 타겟 카운트 (몬스터 n마리, 스테이지 별 n개 이상 ...)
    public int exp { get; set; } // 미션 보상 겸험치
    public int ap { get; set; } // 미션 보상 ap
    public int gold { get; set; } // 미션 보상 골드
    public int diamond { get; set; } // 미션 보상 다이아
    public List<Reward> rewards { get; set; } = new(); // 미션 보상 아이템
    public List<int> nextMissions { get; set; } = new(); // 클리어 시 다음 해금 미션 id
}

public class Reward
{
    public int rewardId { get; }
    public int rewardCount { get; }

    public Reward(int rewardId, int rewardCount)
    {
        this.rewardId = rewardId;
        this.rewardCount = rewardCount;
    }
}