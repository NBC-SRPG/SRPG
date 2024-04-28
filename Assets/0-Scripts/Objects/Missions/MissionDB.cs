using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Constants;

public class MissionDB
{
    private Dictionary<int, MissionSO> missionDic = new();

    public MissionDB()
    {
        List<MissionSO> entities = new();
        
        Addressables.LoadAssetsAsync<MissionSO>("Mission", mission =>
        {
            if (mission != null)
            {
                entities.Add(mission);
            }
        }).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("모든 미션 SO가 성공적으로 로드되었습니다.");

                Managers.UI.FindUI<LoadingUI>().isMissionLoaded = true;

                if (entities == null || entities.Count <= 0)
                {
                    return;
                }

                var entityCount = entities.Count;

                for (int i = 0; i < entityCount; i++)
                {
                    var mission = entities[i];

                    if (missionDic.ContainsKey(mission.missionId))
                    {
                        missionDic[mission.missionId] = mission;
                        Debug.Log(mission.missionId);
                    }
                    else
                    {
                        missionDic.Add(mission.missionId, mission);
                        Debug.Log(mission.missionId);
                    }
                }
            }
            else
            {
                Debug.LogError("미션 SO 로드 실패: " + handle.OperationException);
            }
        };
    }

    public MissionSO Get(int missionId)
    {
        if (missionDic.ContainsKey(missionId))
        {
            return missionDic[missionId];
        }

        return null;
    }
}
