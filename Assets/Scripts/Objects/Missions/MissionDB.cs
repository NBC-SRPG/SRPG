using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;

public class MissionDB
{
    private Dictionary<int, MissionData> missionDic = new();

    public MissionDB()
    {
        List<MissionData> entities = new();

        // TODO
        // DB에서 미션 데이터 가져오기
        // 테스트 데이터
        MissionData missionData = new MissionData();
        missionData.id = 90001000;
        missionData.name = "테스트 미션 1";
        missionData.description = "테스트 미션 1입니다. 80001000 아이템 1개 얻기";
        missionData.missionType = MissionType.GetItem;
        missionData.missionCategory = MissionCategory.Daily;
        missionData.target = 80001000;
        missionData.count = 1;
        missionData.exp = 100;
        missionData.ap = 0;
        missionData.gold = 1000;
        missionData.diamond = 100;
        entities.Add(missionData);

        MissionData missionData1 = new MissionData();
        missionData1.id = 90001001;
        missionData1.name = "테스트 미션 2";
        missionData1.description = "테스트 미션 2입니다. 70001000 몬스터 10마리 잡기";
        missionData1.missionType = MissionType.KillMonster;
        missionData1.missionCategory = MissionCategory.Weekly;
        missionData1.target = 70001000;
        missionData1.count = 10;
        missionData1.exp = 1000;
        missionData1.ap = 5;
        missionData1.gold = 2000;
        missionData1.diamond = 300;
        missionData1.nextMissions.Add(90001002);
        entities.Add(missionData1);

        MissionData missionData2 = new MissionData();
        missionData2.id = 90001002;
        missionData2.name = "테스트 미션 3";
        missionData2.description = "테스트 미션 3입니다. 80001001 아이템 5개 사용";
        missionData2.missionType = MissionType.UseItem;
        missionData2.missionCategory = MissionCategory.Achievement;
        missionData2.target = 80001001;
        missionData2.count = 5;
        missionData2.exp = 5000;
        missionData2.ap = 100;
        missionData2.gold = 5000;
        missionData2.diamond = 700;
        entities.Add(missionData2);

        if (entities == null || entities.Count <= 0)
        {
            return;
        }
            
        var entityCount = entities.Count;

        for (int i = 0; i < entityCount; i++)
        {
            var mission = entities[i];

            if (missionDic.ContainsKey(mission.id))
            {
                missionDic[mission.id] = mission;
            }
            else
            {
                missionDic.Add(mission.id, mission);
            }
        }
    }

    public MissionData Get(int id)
    {
        if (missionDic.ContainsKey(id))
        {
            return missionDic[id];
        }

        return null;
    }
}
