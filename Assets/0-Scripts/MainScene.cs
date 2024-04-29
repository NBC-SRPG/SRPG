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

        Managers.Mission.DailyMissionInit();
        Managers.Mission.WeeklyMissionInit();

        Managers.Mission.NotifyMission(MissionType.Login, 0, 1);
    }
}