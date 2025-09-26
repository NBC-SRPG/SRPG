using UnityEngine;
using static Constants;
public class MainScene : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.ShowUI<MainUI>();
        Managers.UI.ShowUI<CommonUI>();
        Managers.UI.InitSortOrder();
        Debug.Log("MainSceneInit");

        // 일간, 주간 미션 초기화
        Managers.Mission.DailyMissionInit();
        Managers.Mission.WeeklyMissionInit();

        // 로그인
        Managers.Mission.NotifyMission(MissionType.Login, 0, 1);
    }
}